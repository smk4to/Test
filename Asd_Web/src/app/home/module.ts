import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DevExtremeModule } from 'devextreme-angular';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { WindowModule } from '../window/module';

import { HomeRoutingModule } from './routing';

import { HomeComponent } from './component';

@NgModule({
  imports: [CommonModule, DevExtremeModule, HttpClientModule, FormsModule, WindowModule, HomeRoutingModule],
  declarations: [HomeComponent]
})
export class HomeModule { }
