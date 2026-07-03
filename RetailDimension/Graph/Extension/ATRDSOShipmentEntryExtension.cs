using PX.Data;
using PX.Objects.SO;
using System.Collections;
using System.Linq;

namespace RetailDimension.Graph.Extension
{
    public class ATRDSOShipmentEntryExtension : PXGraphExtension<SOShipmentEntry>
    {
        public static bool IsActive() { return true; }

        #region Action Delegates
#if Version23R1 || Version25R1
        public delegate IEnumerable ConfirmShipmentActionDelegate(PXAdapter adapter);

        [PXOverride]
        public virtual IEnumerable ConfirmShipmentAction(PXAdapter adapter, ConfirmShipmentActionDelegate baseMethod)
        {
            baseMethod(adapter);

            if(Base.Document?.Current?.CurrentWorksheetNbr == null) 
                return adapter.Get(); 

            SOPickingWorksheetReview graph = PXGraph.CreateInstance<SOPickingWorksheetReview>();

            graph.worksheet.Current = graph.worksheet.Search<SOPickingWorksheet.worksheetNbr>(Base.Document.Current.CurrentWorksheetNbr);
            //if (graph.worksheet.Current == null) return adapter.Get();

            //SOPickingWorksheet sOPickingWorksheet = PXSelect<
            //    SOPickingWorksheet,
            //    Where<SOPickingWorksheet.worksheetNbr, Equal<Required<SOPickingWorksheet.worksheetNbr>>>>
            //    .Select(Base, Base.Document.Current.CurrentWorksheetNbr);
            //if (sOPickingWorksheet == null) return adapter.Get();


            foreach (SOPickingWorksheetLine line in graph.worksheetLines.Select().RowCast<SOPickingWorksheetLine>().Where(x => x.WorksheetNbr == Base.Document.Current.CurrentWorksheetNbr).ToList())
            {
                //line.PackedQty = line.PickedQty;

                graph.worksheetLines.Update(line);
                graph.Actions.PressSave();

            }

            foreach (SOShipment shipment in graph.shipments.Select().RowCast<SOShipment>().Where(x => x.ShipmentNbr == Base.Document.Current.ShipmentNbr).ToList())
            {
                shipment.PackedQty = shipment.PickedQty;

                graph.shipments.Update(shipment);
                graph.Actions.PressSave();
            }

            graph.worksheet.Current.Status = SOPickingWorksheet.status.Completed;
            graph.worksheet.Update(graph.worksheet.Current);

            //sOPickingWorksheet.Status = SOPickingWorksheet.status.Completed;
            //graph.worksheet.Update(sOPickingWorksheet);

            graph.Actions.PressSave();

            return adapter.Get();
        }
#elif Version25R2
        // In Acumatica 25R2 (25.201+), SOShipmentEntry.ConfirmShipmentAction was changed to a
        // protected virtual method, making [PXOverride] invalid (the framework only resolves public
        // virtual targets). Using it would throw at every SOShipmentEntry instantiation.
        // Equivalent behaviour is achieved via RowUpdated + RowPersisted event handlers.

        private bool _worksheetUpdatePending = false;

        protected virtual void _(Events.RowUpdated<SOShipment> e)
        {
            if (e.Row?.Status == SOShipmentStatus.Confirmed &&
                e.OldRow?.Status != SOShipmentStatus.Confirmed)
            {
                _worksheetUpdatePending = true;
            }
        }

        protected virtual void _(Events.RowPersisted<SOShipment> e)
        {
            if (e.TranStatus != PXTranStatus.Completed) return;
            if (!_worksheetUpdatePending) return;
            _worksheetUpdatePending = false;

            if (Base.Document?.Current?.CurrentWorksheetNbr == null)
                return;

            SOPickingWorksheetReview graph = PXGraph.CreateInstance<SOPickingWorksheetReview>();

            graph.worksheet.Current = graph.worksheet.Search<SOPickingWorksheet.worksheetNbr>(Base.Document.Current.CurrentWorksheetNbr);

            foreach (SOPickingWorksheetLine line in graph.worksheetLines.Select().RowCast<SOPickingWorksheetLine>().Where(x => x.WorksheetNbr == Base.Document.Current.CurrentWorksheetNbr).ToList())
            {
                graph.worksheetLines.Update(line);
                graph.Actions.PressSave();
            }

            foreach (SOShipment shipment in graph.shipments.Select().RowCast<SOShipment>().Where(x => x.ShipmentNbr == Base.Document.Current.ShipmentNbr).ToList())
            {
                shipment.PackedQty = shipment.PickedQty;

                graph.shipments.Update(shipment);
                graph.Actions.PressSave();
            }

            graph.worksheet.Current.Status = SOPickingWorksheet.status.Completed;
            graph.worksheet.Update(graph.worksheet.Current);

            graph.Actions.PressSave();
        }
#endif
        #endregion
    }
}