<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PassiveSegmentBuilderGroupVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Passive Segment Builder</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Passive Segment Builder Products Groups</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">Delete Passive Segment Builder Products Group</th> 
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
                    <td></td>
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
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                        <%= Html.HiddenFor(model => model.ProductGroup.VersionNumber)%>
                        <%= Html.HiddenFor(model => model.ProductGroup.ProductGroupId)%>
                    <%}%>
                    </td>                
               </tr>
            </table>
              
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_passivesegmentbuilder').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>


<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("PSB Products Groups", "Main", new { controller = "PassiveSegmentBuilderGroup", action = "ListUnDeleted", }, new { title = "Passive Segment Builder Products Groups" })%> &gt;
<%=Model.ProductGroup.ProductGroupName%>
</asp:Content>

