package via.sep3.group7.newsdataapi.data;

import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.stereotype.Service;
import via.sep3.group7.newsdataapi.model.DateAndTime;
import via.sep3.group7.newsdataapi.model.News;
import via.sep3.group7.newsdataapi.model.NewsPost;
import via.sep3.group7.newsdataapi.model.PostData;

import java.io.IOException;
import java.util.ArrayList;

@Service
public class InMemoryNewsService implements NewsService {
    ArrayList<News> news = new ArrayList<>();

    public InMemoryNewsService() {
        seed();
    }

    @Override
    public ArrayList<News> getNews(){
        return new ArrayList<>(news);
    }

    @Override
    public void addNews(News news) {
        this.news.add(news);

    }


    private void seed(){
        News n0 = new News(0, "0", "0");
        News n1 = new News(1, "1", "1");
        News n2 = new News(2, "2", "2");
        News n3 = new News(3, "3", "3");
        News n4 = new News(4, "4", "4");
        news.add(n0);
        news.add(n1);
        news.add(n2);
        news.add(n3);
        news.add(n4);
    }

}
