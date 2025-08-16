import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Card } from './models/Card';
import { LanguageSpec } from './models/LanguageSpec';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  public spinning: boolean = false;
  public showDocs: boolean = false;
  public _languageSpec: LanguageSpec | undefined;
  public searchResultList: Card[] = [];
  public searchResultCount: number | undefined;
  public get hasResults(): boolean {
    return this.searchResultCount != undefined;
  }
  public errorText: string | undefined;
  public query: string = "";
  private colorId: string = "";
  public onColorIdUpdated(value: string) {
    this.colorId = value;
  }

  public get hasError(): boolean {
    return this.errorText == undefined;
  }
  public clearError() {
    this.errorText = undefined;
  }
  public onSearchHover() {
    this.clearError();
    if (this.query) {
      this.countSearch();
    }
  }
  public async onSearchClicked() {
    this.showDocs = false;
    if (this.query) {
      await this.fetchSearch();
    }
  }

  private fetchingDocs: Promise<void> | undefined;
  public onDocsHover() {
    if (!this.showDocs && !this._languageSpec) {
      this.fetchingDocs = this.fetchDocs();
    }
  }
  public toggleDocs() {
    if (this.showDocs) {
      this.showDocs = false;
    }
    else {
      if (this.fetchingDocs) {
        this.fetchingDocs.then(x => {
          this.showDocs = true;
          this.fetchingDocs = undefined;
        });
      }
      else {
        this.showDocs = true;
      }
    }
  }


  public get languageSpec(): LanguageSpec {
    return this._languageSpec ?? {
      functionDefinitions: [],
      expressions: undefined,
      isExpressions: undefined
    };
  }
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.fetchDocs();
  }
  async fetchDocs() {
    this.spinning = true;
    await this.executeSearchAndHandleErrors(
      async () => {
        let req = {
          ColorIdentity: this.colorId,
          Query: this.query
        };
        this._languageSpec = await this.http.get<LanguageSpec>('/Documentation/Lang').toPromise();
        //this._languageSpec = await this.http.get<LanguageSpec>('https://localhost:7097/Documentation/Lang').toPromise();
      },
      "failed to fetch documentation, try closing the docs and reopening them"
    );
    this.spinning = false;
  }

  async countSearch() {
    this.spinning = true;
    await this.executeSearchAndHandleErrors(async () => {
      let req = {
        ColorIdentity: this.colorId,
        Query: this.query
      };
      this.searchResultCount = await this.http.post<number>('/Search/CheckSearchCount', req).toPromise();
    });
    this.spinning = false;
  }
  async fetchSearch() {
    this.spinning = true;
    await this.executeSearchAndHandleErrors(async () => {
      let req = {
        ColorIdentity: this.colorId,
        Query: this.query
      };
      this.searchResultList = await this.http.post<Card[]>('/Search/RunSearch', req).toPromise() ?? [];
    });
    this.spinning = false;
  }
  private async executeSearchAndHandleErrors<T>(httpRequest: () => Promise<T>, context?:string) {
    try {
      await httpRequest();
    }
    catch (notSuccess)
    {
      this.searchResultList = [];
      this.searchResultCount = undefined;
      let httpError = notSuccess as HttpErrorResponse;
      if (httpError && httpError.status == 400) {
        this.errorText = "Invalid query: " + httpError.error;
      }
      else if (httpError && httpError.status == 503) {
        //TODO: need some indicator more than 503 that it's a start up error,
        //could be 503 that's not from startup
        this.errorText = "The server is still starting up, currently "+ httpError.error;
      }
      else {
        this.errorText = context??"unknown error occurred, check the console logs with ctrl + shift + J";
        console.log(notSuccess);
      }
    }
  }

  title = 'MTG Search';
}
