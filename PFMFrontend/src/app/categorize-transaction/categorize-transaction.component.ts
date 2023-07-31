import {Component, Inject, OnInit} from '@angular/core';
import {MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {HttpClient} from '@angular/common/http';
import {TransactionService} from "../transaction.service";

@Component({
  selector: 'app-categorize-transaction',
  templateUrl: './categorize-transaction.component.html',
  styleUrls: ['./categorize-transaction.component.css']
})
export class CategorizeTransactionComponent implements OnInit {
  categories: any = {items: []};
  subcategories: any = {items: []};
  selectedCategory: string = '';
  selectedSubcategory: string = '';

  constructor(
    public dialogRef: MatDialogRef<CategorizeTransactionComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private http: HttpClient,
    private transactionService: TransactionService
  ) {
    this.selectedCategory = "";
    this.selectedSubcategory = "";
  }

  ngOnInit(): void {
    this.categories = this.data.categories;
    this.subcategories = [];
    this.getCategories();
  }

  onCategoryChange(cat: string) {
    this.getSubcategories(cat);
  }

  getSubcategories(cat: string): string[] {
    this.subcategories = [];

    // @ts-ignore
    this.data.listSubcategories.forEach((category) => {

      if (cat == category.parentCode) {
        this.subcategories.push(category);
      }
    });
    return this.subcategories;
  }

  onCancelClick() {
    this.dialogRef.close();
  }

  onSaveClick() {
    this.data.category = this.selectedCategory;
    this.data.subcategory = this.selectedSubcategory;
    let requestBody = {
      catcode: ""
    };
    if(this.selectedSubcategory != ""){
      requestBody.catcode = this.selectedSubcategory;
    } else if (this.selectedCategory != ""){
      requestBody.catcode = this.selectedCategory;
    } else {
      return;
    }
    this.transactionService.categorizeTransaction(this.data.id, requestBody).subscribe((transactions) => {
    });
    this.dialogRef.close(this.data);
  }

  getCategories(): void {
    this.categories = this.data.categories;

    // @ts-ignore
    this.categories.forEach((category) => {
      if (this.data.category == category.codeId) {
        this.selectedCategory = category.codeId;
        this.getSubcategories(category.codeId);
      }
    });
    if (this.selectedCategory === "") {
      // @ts-ignore
      this.data.listSubcategories.forEach((subcategory) => {
        if (this.data.category == subcategory.codeId) {
          this.getSubcategories(subcategory.parentCode);
          this.selectedCategory = subcategory.parentCode;
          // @ts-ignore
          this.selectedSubcategory = subcategory.codeId;
        }
      });
    }
  }
}
