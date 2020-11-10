package via.sep3.group7.newsdataapi.controllers;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import via.sep3.group7.newsdataapi.data.NewsService;
import via.sep3.group7.newsdataapi.model.News;
import via.sep3.group7.newsdataapi.model.NewsPost;

import java.io.IOException;
import java.util.ArrayList;

@RestController
@RequestMapping("/news") //localhost:port/news
public class NewsController {
    @Autowired
    NewsService service;

    @GetMapping("")
    public ArrayList<News> getAllNews() {
        return service.getNews();

    }
    @PostMapping("")
    public void addNews(@RequestBody News news){
        service.addNews(news);
    }

}
