﻿using Clickly.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace Clickly.ViewComponents
{
    public class StoriesViewComponent : ViewComponent
    {
        private readonly IStoriesService _storiesService;
        public StoriesViewComponent(IStoriesService storiesService)
        {
            _storiesService = storiesService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allStories =  await _storiesService.GetAllStoriesAsync();
            return View(allStories);
        }
    }
}
