import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Card } from './models/Card';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
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
    this.countSearch();
  }
  constructor(private http: HttpClient) {
  }

  async countSearch() {
    await this.executeSearchAndHandleErrors(async () => {
      let req = {
        ColorIdentity: this.colorId,
        Query: this.query
      };
      this.searchResultCount = await this.http.post<number>('/Search/CheckSearchCount', req).toPromise();
    });
  }
  async fetchSearch() {
    await this.executeSearchAndHandleErrors(async () => {
      let req = {
        ColorIdentity: this.colorId,
        Query: this.query
      };
      this.searchResultList = await this.http.post<Card[]>('/Search/RunSearch', req).toPromise() ?? [];
    });
  }
  private async executeSearchAndHandleErrors<T>(httpRequest: () => Promise<T>) {
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
        this.errorText = "unknown error occurred, check the console logs with ctrl + shift + J";
        console.log(notSuccess);
      }
    }
  }

  title = 'MTG Search';
}
