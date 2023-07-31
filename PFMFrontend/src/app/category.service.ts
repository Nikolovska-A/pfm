import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(
    private http: HttpClient
  ) {
  }

  public getCategories()
  {
    let url = 'http://127.0.0.1:80/v1/categories?pageSize=100';

    return this.http.get(url);
  }
}
