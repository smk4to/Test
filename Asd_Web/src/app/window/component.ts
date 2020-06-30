import { Component, OnInit, Input, Inject } from '@angular/core';
import { AppComponent } from '../component';
import { Router } from '@angular/router';

@Component({ selector: 'app-window', templateUrl: './component.html' })

export class WindowComponent implements OnInit {

  @Input() visible = false;
  @Input() width = 500;
  @Input() height = 400;

  @Input() closeButton = true;

  titleHeight: any;
  contentHeight: any;
  footerHeight: any;

  constructor(@Inject(AppComponent) public app: AppComponent, private router: Router) { }

  ngOnInit() {
    this.titleHeight = '48px';
    this.contentHeight = (this.height - 48 - 28).toString() + 'px';
    this.footerHeight = '28px';
  }

}
