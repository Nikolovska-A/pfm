import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-categorize-transaction',
  templateUrl: './categorize-transaction.component.html',
  styleUrls: ['./categorize-transaction.component.css']
})
export class CategorizeTransactionComponent implements OnInit {
  categories: string[] = [];

  constructor(
    public dialogRef: MatDialogRef<CategorizeTransactionComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    this.getCategories();
  }

  getCategories(): void {
    this.http.get<string[]>('http://your-api-url/categories').subscribe(
      (response: string[]) => {
        this.categories = response;
      },
      (error) => {
        console.error('Error fetching categories:', error);
      }
    );
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }

  onSaveClick(): void {
    this.dialogRef.close();
  }
}
