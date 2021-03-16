<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.TravelerTypeClientDetailVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - ClientDetails
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
   <div id="banner"><div id="banner_text">TravelerType - Client Details</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="2" border="0" width="100%"> 
                <tr> 
	                <th class="row_header" width="100%"><strong>Client Items</strong></th> 
                </tr>  
                <tr> 
                    <td><%= Html.ActionLink("Addresses", "List", "TravelerTypeAddress", new {id = Model.ClientDetail.ClientDetailId, csu = Model.ClientSubUnit.ClientSubUnitGuid, tt=  Model.TravelerType.TravelerTypeGuid }, null)%></td>
                </tr> 
                <tr> 
                    <td><%= Html.ActionLink("Contacts", "List", "TravelerTypeContact", new { id = Model.ClientDetail.ClientDetailId, csu = Model.ClientSubUnit.ClientSubUnitGuid, tt = Model.TravelerType.TravelerTypeGuid }, null)%></td>
                </tr> 	        
                <tr> 
                    <td><%= Html.ActionLink("ESCInformation", "List", "TravelerTypeESCInformation", new { id = Model.ClientDetail.ClientDetailId, csu = Model.ClientSubUnit.ClientSubUnitGuid, tt = Model.TravelerType.TravelerTypeGuid }, null)%></td>
                </tr> 
                <tr> 
                    <td><%= Html.ActionLink("SupplierProducts", "List", "TravelerTypeSupplierProduct", new { id = Model.ClientDetail.ClientDetailId, csu = Model.ClientSubUnit.ClientSubUnitGuid, tt = Model.TravelerType.TravelerTypeGuid }, null)%></td>
                </tr> 	        
                <tr> 
                    <td><%= Html.ActionLink("FormOfPaymentType", "List", "TravelerTypeSubProductFormOfPaymentType", new { id = Model.ClientDetail.ClientDetailId, csu = Model.ClientSubUnit.ClientSubUnitGuid, tt = Model.TravelerType.TravelerTypeGuid }, null)%></td>
                </tr>         
               <tr> 
	                <td class="row_footer"></td> 
                </tr> 
                <tr> 
	                <td class="row_footer_blank"><a href="javascript:history.back();" class="red" title="Back">Back</a></td> 
                </tr>  
            </table> 
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_clientdetails').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>
