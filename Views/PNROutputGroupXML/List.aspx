<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PNROutputGroupLanguage>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - PNR Output Groups
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">PNR Output Remarks - <%=Server.HtmlEncode(Model.LanguageName)  %></div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th width="10%" class="row_header_left">Sequence</th> 
			        <th width="70%">Value</th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                    if (Model.PNROutputGroupXMLDOM != null)
                    {
                        foreach (var item in Model.PNROutputGroupXMLDOM.PNROutputGroupXMLItems)
                        { 
                  
                %>
                <tr>
                    <td><%= Html.Encode(item.Sequence)%></td>
                    <td><%= Html.Encode(item.Value)%></td>
                    <td></td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess")
                          {%>
                        <%=  Html.RouteLink("View", "LanguageView", new { action = "View", id = Model.PNROutputGroupId, languageCode = Model.LanguageCode, node = item.ItemNumber }, new { title = "View Item" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess")
                          {%>
                        <%=  Html.RouteLink("Edit", "LanguageView", new { action = "Edit", id = Model.PNROutputGroupId, languageCode = Model.LanguageCode, node = item.ItemNumber }, new { title = "Edit Item" })%>
                        <%} %>
                    </td>
                   <td>
                        <%if (ViewData["Access"] == "WriteAccess")
                          {%>
                        <%=  Html.RouteLink("Delete", "LanguageView", new { action = "Delete", id = Model.PNROutputGroupId, languageCode = Model.LanguageCode, node = item.ItemNumber }, new { title = "Delete Item" })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                        }
                    }
                %>
                <tr>
                    <td class="row_footer_left"></td>
                    <td colspan="5" class="row_footer_right"></td>
                </tr>
		        <tr> 
		            <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="4"></td> 
		        </tr> 
            </table>
        </div>
    </div>

<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_pnroutputs').click();
		$("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
})
 </script>
</asp:Content>
