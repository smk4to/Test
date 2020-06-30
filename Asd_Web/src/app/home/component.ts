import { Component, OnInit, Inject, HostListener } from '@angular/core';
import { AppComponent } from '../component';
import { Router } from '@angular/router';
import notify from 'devextreme/ui/notify';

@Component({ selector: 'app-home', templateUrl: './component.html' })

export class HomeComponent implements OnInit {

  popupVisible = false;

  height = (document.documentElement.clientHeight - 25) + 'px';
  width = (document.documentElement.clientWidth - 25) + 'px';
  gridHeight = (document.documentElement.clientHeight - 25 - 48 - 28 - 30) + 'px';

  departments: any = [];
  department: any;
  departmentId: any;

  employees: any = [];
  employee: any;
  employeeId: any;

  constructor(@Inject(AppComponent) public app: AppComponent, private router: Router) { }

  @HostListener('window:resize', ['$event'])

  onResize() {
    this.height = (document.documentElement.clientHeight - 25) + 'px';
    this.width = (document.documentElement.clientWidth - 25) + 'px';
    this.gridHeight = (document.documentElement.clientHeight - 25 - 48 - 28 - 30) + 'px';
  }

  ngOnInit() {
    this.GetDepartments();
  }

  onActivate() {
    window.scrollTo(0, 0);
  }

  GetDepartments(): any {
    this.app.loadPanel.visible = true; // показываем "спинер"
    // отправляем запрос
    return this.app.Get('departments', this.departmentId).subscribe((data: any) => {
      this.app.loadPanel.visible = false; // скрываем "спинер"
      console.log(data);
      // проверяем полученные данные
      if (data.Code === 0) {
        // заполняем данные
        if (data.Data) { this.departments = data.Data; }
        // отображаем сообщения, если имеются
        // tslint:disable-next-line: max-line-length
        if (data.Message) { notify({ message: data.Message, position: { my: 'center top', at: 'center top', offset: '0 10' } }, 'success', 3000); }
      } else {
        // отображаем предупреждения, если имеются
        // tslint:disable-next-line: max-line-length
        if (data.Message) { notify({ message: data.Message, position: { my: 'center top', at: 'center top', offset: '0 10' } }, 'error', 3000); }
      }
    }, (error: any) => {
      this.app.loadPanel.visible = false; // скрываем "спинер"
    });
  }

  GetDepartmentEmployees(departmentId): any {
    let temp = null;
    if (this.departments) {
      temp = this.departments.find(item => item.Id === departmentId);
    }
    if (temp) {
      return temp.Employees;
    } else {
      return [];
    }
  }

  ButtonClick(departmentId, employeeId) {
    console.log(departmentId, employeeId);
    if (departmentId) {
      if (this.departments) {
        this.department = this.departments.find(i => i.Id === departmentId);
        if (employeeId) {
          if (this.department.Employees) {
            this.employee = this.department.Employees.find(i => i.Id === employeeId);
            console.log(this.employee);
            this.popupVisible = true;
          } else {
            this.employee = {};
          }
          this.employeeId = employeeId;
        }
      } else {
        this.department = {};
      }
      this.departmentId = departmentId;
    }
  }

  ClosePopup() {
    this.popupVisible = false;
  }


}
