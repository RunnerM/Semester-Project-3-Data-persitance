package via.sep3.group7.newsdataapi.data;

import via.sep3.group7.newsdataapi.model.News;
import via.sep3.group7.newsdataapi.model.NewsPost;

import java.io.IOException;
import java.util.ArrayList;

public interface NewsService {
    ArrayList<News> getNews() ;
    void addNews(News news);
}
