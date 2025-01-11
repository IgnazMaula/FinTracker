using AutoMapper;
using FinTracker.Application.Common;
using FinTracker.Application.DTOs;
using FinTracker.Domain.Entities;
using MediatR;
using FinTracker.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Application.Services;

public class NewsFeedService
{
    private readonly IAlphavantageNewsService _alphavantageNewsService;

    public NewsFeedService(IAlphavantageNewsService alphavantageNewsService)
    {
        _alphavantageNewsService = alphavantageNewsService;
    }

    public async Task<NewsArticle> GetNewsFeedAsync()
    {
        var result = await _alphavantageNewsService.GetNewsFeed();

        return result;

        // Process the news articles if necessary (e.g., filtering, enriching data)
        //var filteredNews = rawNews.Where(news => news.PublishedDate > DateTime.Now.AddDays(-7)).ToList();

        //return filteredNews;
    }
}
