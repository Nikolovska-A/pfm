import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {TransactionService} from "../transaction.service";
import {CategorizeTransactionComponent} from "../categorize-transaction/categorize-transaction.component";
import {MatDialog} from "@angular/material/dialog";
import {CategoryService} from "../category.service";

@Component({
  selector: 'app-transaction-list',
  templateUrl: './transaction-list.component.html',
  styleUrls: ['./transaction-list.component.css']
})
export class TransactionListComponent implements OnInit {
  listTransactions: any = {items: []};
  categories: any = {items: []};
  listSubcategories: any = {items:[]};
  totalPages: number;
  currentPage: number;
  totalPagesShown: number;
  filterKind: any;

  constructor(public transactionService: TransactionService, public dialog: MatDialog, private categoryService: CategoryService) {
    this.listTransactions = [];
    this.totalPages = 10;
    this.currentPage = 1;
    this.totalPagesShown = 10;
    this.filterKind = null;
  }

  public ngOnInit() {
    this.categories = [];
    this.listSubcategories = [];
    this.transactionService.getTransactions(null, null).subscribe((transactions) => {
      // @ts-ignore
      this.totalPages = transactions.totalPages;
      if (this.totalPages > 10) {
        this.totalPagesShown = 10;
      }
      this.totalPagesShown = this.totalPages;
      this.listTransactions = transactions;
    });
    this.categoryService.getCategories().subscribe((categories) => {
      // @ts-ignore
      this.categories = categories.items.filter((category) => category.parentCode === "");
      // @ts-ignore
      this.listSubcategories = categories.items.filter((category) => category.parentCode != "");

    });

  }

  onClickButton(pageNumber: number) {
    this.transactionService.getTransactions(pageNumber, this.filterKind).subscribe((transactions) => {
      this.listTransactions = transactions;
      this.currentPage = pageNumber;
    });
  }

  totalPagesArray(): number[] {
    const maxVisiblePages = 5; // Number of paging buttons to show (can be adjusted)
    const startPage = Math.max(this.currentPage - Math.floor(maxVisiblePages / 2), 1);
    const endPage = Math.min(startPage + maxVisiblePages - 1, this.totalPages);

    return Array.from({length: endPage - startPage + 1}, (_, i) => startPage + i);
  }

  filterByKind() {
    this.transactionService.getTransactions(this.currentPage, this.filterKind).subscribe((transactions) => {
      this.listTransactions = transactions;
    });
  }

  onCategorizeClick(transaction:any): void {
    const dialogRef = this.dialog.open(CategorizeTransactionComponent, {
      width: '400px',
      data: {category: transaction.catCode,categories: this.categories, listSubcategories: this.listSubcategories, id: transaction.transactionId} // Pass any data you want to access in the popup
    });

    // @ts-ignore
    dialogRef.afterClosed().subscribe(result => {
      console.log('Dialog closed:', result);
      this.filterByKind();
    });
  }

  getCategoryName(catCode: number): string {
    // @ts-ignore
    let category = this.categories.find((c) => c.codeId === catCode);

    if(category == null){
      // @ts-ignore
      category = this.listSubcategories.find((c) => c.codeId === catCode);
    }

    return category ? category.name : 'Add Category';

  }


  @ViewChild('buttonRef') buttonRef!: ElementRef;

  onEnterKey(event: any) {
    event.preventDefault();
    this.filterByKind();
    this.buttonRef.nativeElement.blur();
  }

}
