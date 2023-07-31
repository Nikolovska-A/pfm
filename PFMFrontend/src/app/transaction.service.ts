import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  constructor(
    private http: HttpClient
  ) {
  }

  public getTransactions(page: number | null, filterKind:string | null) {
    let url = 'http://127.0.0.1:80/v1/transactions';
    let queryParams:string='';
    if (page !== null) {
      queryParams += `?page=${page}`;
    }
    if(filterKind!=null)
    {
      if(queryParams.length>0){
        queryParams+='&';
      }
      else{
        queryParams+='?';
      }
      queryParams+="transactionKind="+filterKind;
    }
    return this.http.get(url+queryParams);
  }

  public categorizeTransaction(id: number, catcode: {catcode: string}) {
    let url = 'http://127.0.0.1:80/v1/transactions/' + id + '/categorize';
    return this.http.patch(url, new Object(catcode));
  }
  public getStatistics(date:Date, direction:string)
  {
    let url = 'http://127.0.0.1:80/v1/transactions/statistics';
    let queryParams = '?date='+ date.toDateString() + '&direction='+direction;

    return this.http.get(url+queryParams);
  }

}
