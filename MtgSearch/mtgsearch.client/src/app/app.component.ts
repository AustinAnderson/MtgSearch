import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Card } from './models/Card';

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  public forecasts: WeatherForecast[] = [];
  public testData: Card;

  constructor(private http: HttpClient) {
    this.testData = new Card();
    this.testData.Name = "Sisay, Weatherlight Captain";
    this.testData.ManaCost = "{2}{W}";
    this.testData.SuperTypes = ['Legendary'];
    this.testData.Types = ['Creature'];
    this.testData.SubTypes = ['Human', 'Soldier'];
    this.testData.Power = '2';
    this.testData.Toughness = '2';
    //as if user searched for reg(text,'you control.*search')
    this.testData.TextLines = [
      {
        Segments: [
          { IsHighlighted: false, IsSymbol: false, Text: 'Sisay, Weatherlight Captain gets +1/+1 for each color among other legendary permanents ' },
          { IsHighlighted: true, IsSymbol: false, Text: 'you control.' },
        ]
      },
      {
        Segments: [
          { IsHighlighted: false, IsSymbol: true, Text: '{W}' },
          { IsHighlighted: false, IsSymbol: true, Text: '{U}' },
          { IsHighlighted: false, IsSymbol: true, Text: '{B}' },
          { IsHighlighted: false, IsSymbol: true, Text: '{R}' },
          { IsHighlighted: false, IsSymbol: true, Text: '{G}' },
          { IsHighlighted: true, IsSymbol: false, Text: ': Search' },
          { IsHighlighted: false, IsSymbol: false, Text: " your library for a legendary permanent card with converted mana cost less than Sisay's power, put that card onto the battlefield, then shuffle your library." },
        ]
      }
    ];

  }

  ngOnInit() {
    this.getForecasts();
  }

  getForecasts() {
    this.http.get<WeatherForecast[]>('/weatherforecast').subscribe(
      (result) => {
        this.forecasts = result;
      },
      (error) => {
        console.error(error);
      }
    );
  }

  title = 'mtgsearch.client';
}
