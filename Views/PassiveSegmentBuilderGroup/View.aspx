<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PassiveSegmentBuilderGroupVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Passive Segment Builder</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
    <link href="<%=Url.Content("~/Style/tablesorter/blue/style.css")%>" rel="stylesheet" type="text/css" /> 
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Passive Segment Builder Products Groups</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <td class="row_header" colspan="3">View Passive Segment Builder Products Group</td> 
		        </tr> 
                  <tr>
                     <td>Group Name</td>
                    <td><%= Html.Encode(Model.ProductGroup.ProductGroupName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Enabled?</td>
                    <td><%= Html.Encode(Model.ProductGroup.EnabledFlag)%></td>
                    <td></td>
                </tr>  
               <tr>
                    <td>Enabled Date</td>
                    <td><%= Html.Encode(Model.ProductGroup.EnabledDate.HasValue ? Model.ProductGroup.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Expiry Date</td>
                    <td><%= Html.Encode(Model.ProductGroup.ExpiryDate.HasValue ? Model.ProductGroup.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Deleted?</td>
                    <td><%= Html.Encode(Model.ProductGroup.DeletedFlag)%></td>
                    <td></td>
                </tr>  
                <tr>
                    <td>Deleted Date/Time</td>
                    <td><%= Html.Encode(Model.ProductGroup.DeletedDateTime)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Hierarchy Type</td>
                    <td><%= Html.Encode(Model.ProductGroup.HierarchyType)%></td>
                    <td></td>
                </tr>
                 <tr>
                    <td><%= Html.Label(Model.ProductGroup.HierarchyType)%></td>
                    <td><%= Html.Encode(Model.ProductGroup.HierarchyItem)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><b>Products</b></td>
                    <td></td>
                    <td></td>
                </tr>
                <%
                bool ShowOther = false;
                foreach(SelectListItem x in Model.Products)
                {
                    if (x.Value.ToString() == "13" && x.Selected)
                    {
                        ShowOther = true;
                    } 
                %>                  
                <tr>
                    <td><%=x.Text%></td>
                    <td><%=x.Selected%></td>
                    <td> </td>
                </tr>
                <%
                }
                if (ShowOther && Model.SubProducts.Count > 0)
                { %>
                <tr>
                    <td><b>Sub Products</b></td>
                    <td></td>
                    <td></td>
                </tr>
                <%
                foreach(SelectListItem x in Model.SubProducts)
                {        
                %>                  
                <tr>
                    <td><%=x.Text%></td>
                    <td> </td>
                    <td> </td>
                </tr>
                <%
                }
                
                }
                %>
                 <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                 <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                    <td class="row_footer_blank_right" ></td>
                </tr>
            </table>
        </div>
    </div>

<script type="text/javascript">
    $(document).ready(function() {
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#menu_passivesegmentbuilder').click();
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("PSB Products Groups", "Main", new { controller = "PassiveSegmentBuilderGroup", action = "ListUnDeleted", }, new { title = "Passive Segment Builder Products Groups" })%> &gt;
<%=Model.ProductGroup.ProductGroupName%>
</asp:Content>