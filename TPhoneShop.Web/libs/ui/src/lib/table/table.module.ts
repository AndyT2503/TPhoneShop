import { NgModule } from '@angular/core';
import {
  TbCenterDirective,
  TbLeftDirective,
  TbRightDirective,
} from './table-align/table-align.directive';
import { TableComponent } from './table.component';

@NgModule({
  imports: [
    TableComponent,
    TbCenterDirective,
    TbLeftDirective,
    TbRightDirective,
  ],
  exports: [
    TableComponent,
    TbCenterDirective,
    TbLeftDirective,
    TbRightDirective,
  ],
})
export class TableModule {}
