import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DevExtremeModule } from 'devextreme-angular';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { WindowModule } from './window/module';

import { AppRoutingModule } from './routing';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './component';

@NgModule({
  imports: [CommonModule, DevExtremeModule, HttpClientModule, FormsModule, WindowModule, AppRoutingModule, BrowserModule],
  declarations: [AppComponent],
  bootstrap: [AppComponent],
})
export class AppModule { }
