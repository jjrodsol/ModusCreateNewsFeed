using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using ModusCreateNewsFeed.EF;
using System.Data.Entity;
using System.Transactions;

namespace ModusCreateNewsFeed.Feeds
{
    public partial class Index : System.Web.UI.Page
    {
        private MCNewsFeedDBEntities context = new MCNewsFeedDBEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindGridView();

                    selCategory.DataSource = context.Categories.OrderBy(x => x.Name).ToList();
                    selCategory.DataTextField = "Name";
                    selCategory.DataValueField = "CategoryID";
                    selCategory.DataBind();
                }
            }
            catch (Exception ex)
            { ShowException(ex.Message); }
        }

        private void BindGridView()
        {
            try
            {
                this.feedsGrid.DataSource = context.FeedsView.OrderBy(x => x.Name).ToList();
                this.feedsGrid.DataBind();
            }
            catch (Exception ex)
            {
                ShowException(ex.Message);
            }
        }

        protected void feedsGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                feedsGrid.EditIndex = e.NewEditIndex;
                this.BindGridView();

            }
            catch (Exception ex)
            { ShowException(ex.Message); }
        }

        protected void feedsGrid_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = feedsGrid.Rows[e.RowIndex];

                Guid feedID = new Guid(((Label)row.FindControl("lblFeedID")).Text);
                string name = (row.FindControl("txtName") as TextBox).Text;
                Guid categoryID = new Guid((row.FindControl("ddlCategories") as DropDownList).SelectedValue);
                string description = (row.FindControl("txtDescription") as TextBox).Text;
                string url = (row.FindControl("txtUrl") as TextBox).Text;

                if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(url))
                {
                    string script = "alert(\"The name and url are required!\");";
                    ScriptManager.RegisterStartupScript(this, GetType(),
                                          "ServerControlScript", script, true);
                    return;
                }

                Feed feed = new Feed
                {
                    FeedID = feedID,
                    Name = name,
                    Description = description,
                    Url = url,
                    CategoryID = categoryID
                };

                context.Entry(feed).State = EntityState.Modified;
                context.SaveChanges();

                feedsGrid.EditIndex = -1;
                this.BindGridView();
            }
            catch (Exception ex)
            { ShowException(ex.Message); }
        }

        protected void feedsGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                feedsGrid.EditIndex = -1;
                this.BindGridView();
            }
            catch (Exception ex)
            { ShowException(ex.Message); }
        }

        protected void feedsGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)feedsGrid.Rows[e.RowIndex];
                Guid feedID = new Guid(((Label)row.FindControl("lblFeedID")).Text);

                using (TransactionScope transaction = new TransactionScope())
                {
                    context.Articles.RemoveRange(context.Articles.Where(x => x.FeedID == feedID));
                    context.SaveChanges();

                    Feed feed = context.Feeds.Find(feedID);
                    context.Entry(feed).State = EntityState.Deleted;
                    context.SaveChanges();

                    transaction.Complete();
                }
                

                    

                BindGridView();
            }
            catch (Exception ex)
            { ShowException(ex.Message); }
        }

        protected void feedsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                    {
                        DropDownList dropDownList = (DropDownList)e.Row.FindControl("ddlCategories");

                        dropDownList.DataSource = context.Categories.OrderBy(x => x.Name).ToList();
                        dropDownList.DataTextField = "Name";
                        dropDownList.DataValueField = "CategoryID";
                        dropDownList.DataBind();

                        FeedView feedView = (FeedView)e.Row.DataItem;
                        dropDownList.SelectedValue = feedView.CategoryID.ToString();
                    }
                }
            }
            catch (Exception ex)
            { ShowException(ex.Message); }
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {

                    Feed feed = new Feed
                    {
                        Name = txtNameDialog.Value,
                        Description = txtDescriptionDialog.Value,
                        Url = txtUrlDialog.Value,
                        CategoryID = new Guid(selCategory.Value)
                    };

                    context.Feeds.Add(feed);
                    context.SaveChanges();

                    BindGridView();

                    txtNameDialog.Value = string.Empty;
                    txtDescriptionDialog.Value = string.Empty;
                    txtUrlDialog.Value = string.Empty;
                    selCategory.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            { ShowException(ex.Message); }
        }

        private void ShowException(string message)
        {
            string script = String.Format("alert(\"{0}!\");", message);
            ScriptManager.RegisterStartupScript(this, GetType(),
                                  "ServerControlScript", script, true);
        }
    }
}