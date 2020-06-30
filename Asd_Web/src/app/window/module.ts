import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DevExtremeModule } from 'devextreme-angular';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { WindowComponent } from './component';

@NgModule({
  imports: [CommonModule, DevExtremeModule, HttpClientModule, FormsModule],
  declarations: [WindowComponent],
  exports: [WindowComponent]
})
export class WindowModule { }
