import {NgModule} from '@angular/core';
import {RouterModule, Routes} from "@angular/router";
import {TransactionListComponent} from "./transaction-list/transaction-list.component";

const routes: Routes = [
  {
  path: 'transaction-list',
  component: TransactionListComponent
}
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
