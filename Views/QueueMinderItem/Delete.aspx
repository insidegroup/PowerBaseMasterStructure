<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.QueueMinderItemVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Queues</asp:Content>


<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Follow Up Queue Items</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		          <tr> 
			        <th class="row_header" colspan="3">Delete Follow Up Queue Item</th> 
		        </tr> 
                 <tr>
                <td>Description</td>
                <td><%= Html.Encode(Model.QueueMinderItem.QueueMinderItemDescription)%></td>
                <td></td>
            </tr> 
             <tr>
                <td>Type</td>
                <td><%= Html.Encode(Model.QueueMinderItem.QueueMinderType.QueueMinderTypeDescription)%></td>
                <td></td>
            </tr>  
             <tr>
                <td>Pseudo City Or Office Id</td>
                <td><%= Html.Encode(Model.QueueMinderItem.PseudoCityOrOfficeId)%></td>
                <td></td>
            </tr> 
             <tr>
                <td>Queue Number</td>
                <td><%= Html.Encode(Model.QueueMinderItem.QueueNumber)%></td>
                <td></td>
            </tr> 
             <tr>
                <td>Queue Category</td>
                <td><%= Html.Encode(Model.QueueMinderItem.QueueCategory)%></td>
                <td></td>
            </tr>   
             <tr>
                <td>Queue PrefactoryCode</td>
                <td><%= Html.Encode(Model.QueueMinderItem.PrefactoryCode)%></td>
                <td></td>
            </tr>   
             <tr>
                <td>GDS</td>
                <td><%= Html.Encode(Model.QueueMinderItem.GDS.GDSName)%></td>
                <td></td>
            </tr>   
             <tr>
                <td>Context</td>
                <td><%= Html.Encode(Model.QueueMinderItem.Context.ContextName)%></td>
                <td></td>
            </tr>        
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right" colspan="2">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                        <%= Html.HiddenFor(model => model.QueueMinderItem.QueueMinderItemId)%>
                        <%= Html.HiddenFor(model => model.QueueMinderItem.VersionNumber)%>
                    <%}%>
                    </td>                
               </tr>
            </table>
            
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_ticketqueuegroups').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

