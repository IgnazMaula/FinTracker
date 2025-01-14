using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Application.Models;

public class NewsArticle
{
    public int Items { get; set; }
    public string SentimentScoreDefinition { get; set; }
    public string RelevanceScoreDefinition { get; set; }
    public List<NewsItem> Feed { get; set; }
}

public class NewsItem
{
    public string Title { get; set; }
    public string Url { get; set; }
    public string TimePublished { get; set; }
    public List<string> Authors { get; set; }
    public string Summary { get; set; }
    public string BannerImage { get; set; }
    public string Source { get; set; }
    public string CategoryWithinSource { get; set; }
    public string SourceDomain { get; set; }
    public List<NewsTopic> Topics { get; set; }
    public double OverallSentimentScore { get; set; }
    public string OverallSentimentLabel { get; set; }
    public List<TickerSentiment> TickerSentiment { get; set; }
}

public class NewsTopic
{
    public string Topic { get; set; }
    public double RelevanceScore { get; set; }
}

public class TickerSentiment
{
    public string Ticker { get; set; }
    public double RelevanceScore { get; set; }
    public double TickerSentimentScore { get; set; }
    public string TickerSentimentLabel { get; set; }
}

