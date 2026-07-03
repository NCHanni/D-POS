<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="ICDH4011.aspx.cs" Inherits="Page_ICDH4011" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        PrimaryView="Document" TypeName="RetailDimension.Graph.ICDHPOSCreditMemoInq">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phL" runat="Server">
    <px:PXGrid ID="grid" runat="server" Height="400px" Width="100%" Style="z-index: 100"
        AllowPaging="True" AllowSearch="true" AdjustPageSize="Auto" DataSourceID="ds" SkinID="PrimaryInquire" SyncPosition="True">
        <Levels>
            <px:PXGridLevel DataKeyNames="DocType,RefNbr" DataMember="Document">
                <Columns>
                    <px:PXGridColumn DataField="RefNbr" Width="150px" LinkCommand="ViewDocument" />
                    <px:PXGridColumn DataField="Status" Width="100px" />
                    <px:PXGridColumn DataField="DocDate" Width="90px" />
                    <px:PXGridColumn DataField="DocDesc" Width="200px" />
                    <px:PXGridColumn DataField="CuryOrigDocAmt" Width="120px" TextAlign="Right" />
                    <px:PXGridColumn DataField="DocType" Width="100px" />
                    <px:PXGridColumn DataField="CustomerID" Width="120px" />
                    <px:PXGridColumn DataField="Customer__AcctName" Width="200px" />
                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="130px" Visible="False" />
                </Columns>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="200" />
        <Mode AllowAddNew="False" AllowDelete="False" AllowUpdate="False" />
    </px:PXGrid>
</asp:Content>
