<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ModusCreateNewsFeed.Feeds.Index" %>
<asp:Content ID="ContentBody" ContentPlaceHolderID="MainContent" runat="server"> 
    <div class="default-main-div">
        <ol class="breadcrumb">
          <li class="breadcrumb-item"><a href="/Default">Home</a></li>
          <li class="breadcrumb-item active">Feeds</li>
        </ol>
        <h2>Feeds</h2> 
        <hr />
        <div id="toolbar">
            <ul class="nav nav-pills">              
              <li class="nav-item">
                <a id="addLink" class="nav-link" href="#" data-toggle="modal" data-target="#addModal"><span class="fa fa-plus">&nbsp;</span>Add new</a>
              </li>              
            </ul>
        </div>
        <div>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView runat="server" ID="feedsGrid" DataKeyNames="FeedID" AutoGenerateColumns="False" Width="100%" CssClass="table" 
                        AllowPaging="true" PageSize="25" OnRowEditing="feedsGrid_RowEditing" OnRowUpdating="feedsGrid_RowUpdating" 
                        OnRowCancelingEdit="feedsGrid_RowCancelingEdit" OnRowDeleting="feedsGrid_RowDeleting" OnRowDataBound="feedsGrid_RowDataBound" 
                        EmptyDataText="No feeds has been added.">
                        <HeaderStyle CssClass="grid-header" />
                        <Columns>          
                            <asp:TemplateField ShowHeader="false" ItemStyle-Width="6%" ItemStyle-HorizontalAlign="Center">
	                            <ItemTemplate>
		                            <asp:LinkButton runat="server" CommandName="Edit" ToolTip="Edit" CausesValidation="false">
                                        <i class="fa fa-pencil"></i>
                                    </asp:LinkButton>                                           
	                            </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" CommandName="Update" ToolTip="Update" CausesValidation="false">
                                        <i class="fa fa-check"></i>
                                    </asp:LinkButton>   
                                    <asp:LinkButton runat="server" CommandName="Cancel" ToolTip="Cancel" CausesValidation="false">
                                        <i class="fa fa-times"></i>
                                    </asp:LinkButton>                                       
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="false" ItemStyle-Width="6%" ItemStyle-HorizontalAlign="Center">
	                            <ItemTemplate>
		                            <asp:LinkButton runat="server" CommandName="Delete" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete this feed and its articles?');" CausesValidation="false">
                                        <i class="fa fa-trash"></i>
                                    </asp:LinkButton>                                           
	                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FeedID" SortExpression="Name" Visible="false">             
                                <ItemTemplate> 
                                    <asp:Label ID="lblFeedID" runat="server" Text='<%# Bind("FeedID") %>'/>
                                </ItemTemplate>                               
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="Name" SortExpression="Name" ItemStyle-Width="12%">             
                                <ItemTemplate> 
                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'/>
                                </ItemTemplate> 
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name") %>' MaxLength="50"/>
                                </EditItemTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="Category Name" SortExpression="CategoryName" ItemStyle-Width="15%">              
                                <ItemTemplate> 
                                    <asp:Label ID="lblCategoryName" runat="server" Text='<%# Bind("CategoryName") %>'/>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlCategories" runat="server" >
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="Description" SortExpression="Description" ItemStyle-Width="36%">              
                                <ItemTemplate> 
                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'/>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDescription" runat="server" Text='<%# Eval("Description") %>' MaxLength="1000"/>
                                </EditItemTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="Url" SortExpression="Url" ItemStyle-Width="25%">              
                                <ItemTemplate> 
                                    <asp:Label ID="lblUrl" runat="server" Text='<%# Bind("Url") %>'/>
                                </ItemTemplate> 
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtUrl" runat="server" Text='<%# Eval("Url") %>' MaxLength="500"/>
                                </EditItemTemplate>
                            </asp:TemplateField>                            
                        </Columns> 
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div> 
    <div class="modal fade" id="addModal" style="display:none">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Adding a new feed</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body default-main-div">
                    <div class="form-group">
                        <label class="col-form-label" for="txtNameDialog">Name</label>                        
                        <input type="text" class="form-control" placeholder="Feed name" id="txtNameDialog" runat="server" maxlength="50"/> 
                        <asp:RequiredFieldValidator runat="server" id="reqName" controltovalidate="txtNameDialog" errormessage="The name of the feed is required!" />
                    </div>
                    <div class="form-group">
                        <label class="col-form-label" for="txtDescriptionDialog">Description</label>
                        <textarea class="form-control" id="txtDescriptionDialog" rows="3" runat="server" maxlength="1000"></textarea>                        
                    </div>
                    <div class="form-group">
                        <label class="col-form-label" for="txtUrlDialog">Url</label>                        
                        <input type="text" class="form-control" placeholder="Feed url" id="txtUrlDialog" runat="server" maxlength="500"/>   
                        <asp:RequiredFieldValidator runat="server" id="reqUrl" controltovalidate="txtNameDialog" errormessage="The url is required!" />
                    </div>
                    <div class="form-group">
                        <label class="col-form-label" for="selCategory">Category</label>                        
                        <select class="form-control" id="selCategory" runat="server">
                        </select>                        
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="submit" class="btn btn-primary" runat="server" id="btnSave" onserverclick="btnSave_ServerClick">Save</button>
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    
 
<script type="text/javascript">
    $(function () {
        $('#btnSave').click(function () {
           $('#addModal').modal('hide');
        });
    });
</script>
</asp:Content>
