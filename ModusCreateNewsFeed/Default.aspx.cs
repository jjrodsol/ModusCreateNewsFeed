using ModusCreateNewsFeed.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.ServiceModel.Syndication;
using System.Web.Services;

namespace ModusCreateNewsFeed
{
    public partial class _Default : Page
    {
        private MCNewsFeedDBEntities context = new MCNewsFeedDBEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    GetFeedsCategories();
                    GetFeedsNews();
                }

            }
            catch (Exception ex)
            { ShowException(ex.Message); }
        }

        private void GetFeedsNews()
        {
            try
            {
                XmlReader xmlReader;
                SyndicationFeed syndicationFeed = new SyndicationFeed();
                List<FeedView> feeds = context.FeedsView.ToList();

                context.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.[Articles]");                

                foreach (FeedView feed in feeds)
                {
                    xmlReader = XmlReader.Create(feed.Url);
                    syndicationFeed = SyndicationFeed.Load(xmlReader);
                    xmlReader.Close();

                    List<Article> articles = new List<Article>();

                    foreach (SyndicationItem item in syndicationFeed.Items)
                    {
                        Article article = new Article {
                            Author = item.Authors.Count > 0 ? item.Authors.FirstOrDefault().Name : String.Empty,
                            DatePublished = item.PublishDate.DateTime,
                            FeedID = feed.FeedID,
                            Link = item.Links.Count > 0 ? item.Links.FirstOrDefault().Uri.AbsoluteUri : String.Empty,
                            Summary = item.Summary.Text,
                            Title = item.Title.Text
                        };

                        articles.Add(article);
                    }

                    context.Articles.AddRange(articles);
                    context.SaveChanges();
                }
                ShowFeedsNews();
            }
            catch(Exception ex)
            { ShowException(ex.Message); }
        }

        private void GetFeedsCategories()
        {
            try
            {
                TreeNode allNode = new TreeNode() {
                    Text = "All",
                    Value = string.Empty
                };

                tvFeeds.Nodes.Add(allNode);

                List<FeedView> feeds = context.FeedsView.ToList();    
                var categories = feeds.Select(m => new { m.CategoryID, m.CategoryName }).Distinct().ToList();

                foreach (var category in categories)
                {
                    TreeNode categoryNode = new TreeNode() {
                        Text = category.CategoryName,
                        Value = category.CategoryID.ToString()
                    };

                    foreach (var feed in feeds.Where(x => x.CategoryID == category.CategoryID))
                    {
                        TreeNode feedNode = new TreeNode()
                        {
                            Text = feed.Name,
                            Value = feed.FeedID.ToString()
                        };

                        categoryNode.ChildNodes.Add(feedNode);
                    }

                    tvFeeds.Nodes.Add(categoryNode);
                }
            }
            catch (Exception ex)
            { ShowException(ex.Message); }
        }

        private void ShowFeedsNews()
        {
            try
            {
                Guid? id = null;
                string searchText = txtSearch.Value;

                if (tvFeeds.SelectedValue != string.Empty)
                    id = new Guid(tvFeeds.SelectedValue);

                List<ArticleView> articles = new List<ArticleView>();

                if (id.HasValue)
                {
                    if(tvFeeds.SelectedNode.Parent != null)
                        articles = context.ArticlesView.Where(x => x.FeedID == id).ToList();
                    else
                        articles = context.ArticlesView.Where(x => x.CategoryID == id).ToList();
                }
                else
                    articles = context.ArticlesView.ToList();

                if (!string.IsNullOrEmpty(searchText))
                    articles = articles.Where(x => x.Title.ToLower().Contains(searchText.ToLower()) || x.Summary.ToLower().Contains(searchText.ToLower())).ToList();

                articles = articles.OrderByDescending(x => x.DatePublished).ToList();

                lblCount.InnerText = string.Format("Showing {0} results", articles.Count);

                rptNews.DataSource = articles;
                rptNews.DataBind();
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

        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                ShowFeedsNews();
            }
            catch (Exception ex)
            { ShowException(ex.Message); }
        }

        protected void tvFeeds_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                txtSearch.Value = string.Empty;
                ShowFeedsNews();
            }
            catch (Exception ex)
            { ShowException(ex.Message); }
        }
    }
}