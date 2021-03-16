<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PassiveSegmentBuilderGroupVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Passive Segment Builder</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
    <link href="<%=Url.Content("~/Style/tablesorter/blue/style.css")%>" rel="stylesheet" type="text/css" /> 
    
    <script src="<%=Url.Content("~/Scripts/ERD/ProductGroup.js")%>" type="text/javascript"></script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="contentarea">
    <div id="banner"><div id="banner_text">Passive Segment Builder Products Groups</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Passive Segment Builder Products Group</th> 
		        </tr> 
		         <tr>
                    <td>PSB Group Name</td>
                    <td><label id="lblAuto"></label></td>
                    <td><%= Html.HiddenFor(model => model.ProductGroup.ProductGroupName)%><label id="lblProductGroupNameMsg"/></td>
                </tr>  
                <tr>
                    <td><label for="ProductGroup_EnabledFlagNonNullable">Enabled?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.ProductGroup.EnabledFlagNonNullable)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ProductGroup.EnabledFlagNonNullable)%></td>
                </tr>  
                <tr>
                    <td><label for="ProductGroup_EnabledDate">Enabled Date</label></td>
                    <td><%= Html.EditorFor(model => model.ProductGroup.EnabledDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ProductGroup.EnabledDate)%></td>
                </tr> 
                <tr>
                    <td><label for="ProductGroup_ExpiryDate">Expiry Date</label> </td>
                    <td> <%= Html.EditorFor(model => model.ProductGroup.ExpiryDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ProductGroup.ExpiryDate)%></td>
                </tr>
                  <tr>
                    <td><label for="ProductGroup_InheritFromParentFlag">Inherit From Parent?</label></td>
                     <td><%= Html.CheckBoxFor(model => model.ProductGroup.InheritFromParentFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ProductGroup.InheritFromParentFlag)%></td>
                </tr> 
                <tr>
                    <td><label for="ProductGroup_HierarchyType">Hierarchy Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.ProductGroup.HierarchyType, Model.HierarchyTypes, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ProductGroup.HierarchyType)%></td>
                </tr>           
                <tr>
                    <td><label id="lblHierarchyItem"/>HierarchyItem</td>
                    <td> <%= Html.TextBoxFor(model => model.ProductGroup.HierarchyItem, new { disabled="disabled",  size = "30" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.ProductGroup.HierarchyItem)%>
                        <%= Html.HiddenFor(model => model.ProductGroup.HierarchyCode)%>
                        <label id="lblHierarchyItemMsg"/>
                    </td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
            </table>
            <table id="products" class="tablesorter_other2" cellspacing="0" width="100%">
                
            <thead>
                <tr>
                    <th width="20%">Product</th>
                    <th>Select</th>
                </tr>
            </thead>
            <tbody>
                <%
              for (int i = 0; i < Model.Products.Count; i++)
    {        
       %>                  
                    <tr>
                    	<td><%=Model.Products[i].Text%></td>
                    	<td><%=Html.HiddenFor(x => x.Products[i].Value) %><%=Html.CheckBoxFor(m => Model.Products[i].Selected, new { name = Model.Products[i].Value,  text = Model.Products[i].Value })%></td>
                
              
           </tr>
                    <%
    }

                %>
            </tbody>
                
            </table>
               <table id="subproducts" class="tablesorter_other2" cellspacing="0" width="100%"  style="display: none;">
                
            <thead>
                <tr>
                    <th width="30%">Sub-Products</th>
                    <th width="10%"></th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Available</td>
                    <td></td>
                    <td>Current</td>
                </tr>
                <tr>
                    <td><%= Html.ListBoxFor(x => x.AvailableSubProducts, Model.AvailableSubProducts, new {@style="width:250px;height:300px"})%></td>
                    <td align="center">Remove<br /><input type="button" value="<<" id="btnMoveToAvailable"/><br /><br /><br />Add<br /><input type="button" value=">>" id="btnMoveToCurrent" /></td>
                    <td><%= Html.ListBoxFor(x => x.SubProducts, Model.SubProducts, new { @style = "width:250px;height:300px" })%></td>
                </tr>
            </tbody>
            </table>
             <table border="0" cellspacing="0" width="100%">
            <tr>
                    <td class="row_footer_blank_left" width="20%"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create PSB Products Group" class="red" title="Create Passive Segment Builder Products Group"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.ProductGroup.ProductGroupId)%>
            <%= Html.HiddenFor(model => model.ProductGroup.SourceSystemCode)%>
    <% } %>
        </div>
    </div>
</asp:Content>
