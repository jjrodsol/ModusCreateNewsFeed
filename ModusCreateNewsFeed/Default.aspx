<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ModusCreateNewsFeed._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="default-main-div">
                <div class="row">
                    <div class="col-md-4">
                        <h2>Feeds</h2>
                        <asp:TreeView  
                          ID="tvFeeds"
                          ExpandDepth="0" 
                          PopulateNodesFromClient="true"
                          ShowLines="true" 
                          ShowExpandCollapse="true" 
                          runat="server" 
                          OnSelectedNodeChanged="tvFeeds_SelectedNodeChanged" 
                          RootNodeStyle-CssClass="h5" 
                          NodeStyle-CssClass="h5" 
                          LeafNodeStyle-CssClass="h6"/>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group row">
                            <input type="text" class="form-control col-sm-10" placeholder="Search" id="txtSearch" runat="server"/> 
                            <button type="button" class="btn btn-primary col-sm-2" id="btnSearch" runat="server" onserverclick="btnSearch_ServerClick">Search</button>
                        </div>
                        <div>
                            <asp:Repeater ID="rptNews" runat="server">
                                <ItemTemplate>
                                    <div class="card border-info mb-3" style="max-width: 20rem;">
                                      <div class="card-header"><%#Eval("FeedName") %></div>
                                      <div class="card-body">
                                        <h4 class="card-title"><%#Eval("Title") %></h4>                                    
                                        <p class="card-text"><%#Eval("Summary") %></p>
                                          <h6 class="card-title"><%#Eval("Author") %></h6>
                                          <h6 class="card-title"><%#Eval("DatePublished") %></h6>
                                          <a class="card-title" target="_blank" rel="noreferrer" href=<%#Eval("Link") %><><%#Eval("Link") %></a>
                                      </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>                    
                        </div>
                    </div>
                    <div class="col-md-4">
                        <h5 id="lblCount" runat="server"></h5>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
<script type="text/javascript">
</script>
</asp:Content>

