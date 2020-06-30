import { Component, ViewChild } from '@angular/core';
import { DxLoadPanelComponent } from 'devextreme-angular';
import { loadMessages, locale } from 'devextreme/localization';
import ruMessages from 'devextreme/localization/messages/ru.json';
import * as moment from 'moment';
import 'moment/locale/ru';
import { Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import notify from 'devextreme/ui/notify';
import { map, catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Component({ selector: 'app-root', templateUrl: './component.html' })

export class AppComponent {

  @ViewChild('loadPanel', { static: false }) loadPanel: DxLoadPanelComponent;

  correlationKey = 'correlationId';
  apiUrl: string;

  constructor(private httpClient: HttpClient, private router: Router) {
    loadMessages(ruMessages); locale('ru');
    try { this.GetConfig().subscribe(data => { this.apiUrl = data.apiUrl; }); } catch { }
  }

  // получение конфига
  GetConfig(): Observable<any> {
    try {
      return this.httpClient.get('./../../assets/app.config.json');
    } catch { return null; }
  }

  // получение адреса сервера
  GetApiUrl(): any {
    if (this.apiUrl) {
      return this.apiUrl;
    } else {
      return 'https://localhost';
    }
  }

  // создание заголовков для запроса к API без авторизации
  GetApiHeaders(): HttpHeaders {
    try {
      let headers = new HttpHeaders();
      headers = headers.set('Content-Type', 'application/json; charset=utf-8');
      if (this.GetСorrelationId()) { headers = headers.append('CorrelationId', this.GetСorrelationId()); }
      headers = headers.append('Access-Control-Allow-Origin', '*');
      return headers;
    } catch { return null; }
  }

  // сохранение correlationId на стороне клиента
  SetСorrelationId(correlationId: any): any {
    try {
      localStorage.setItem(this.correlationKey, correlationId);
    } catch { }
  }

  // получение correlationId на стороне клиента
  GetСorrelationId(): string {
    try {
      return localStorage.getItem(this.correlationKey);
    } catch { return null; }
  }

  // удаление correlationId на стороне клиента
  DeleteСorrelationId(): any {
    try {
      localStorage.removeItem(this.correlationKey);
    } catch { }
  }

  // обработка ошибки запроса к серверу и формирование сообщения
  GetError(error: any): any {
    const asdError = Object();
    asdError.Status = error.status;
    try {
      switch (asdError.Status) {
        case 0:
          asdError.Message = 'Ошибка при обращении к серверу. Сервер недоступен.';
          return asdError;
        case 401:
          asdError.Message = 'Ошибка при обращении к серверу. Пользователь не авторизован.';
          return asdError;
        case 403:
          asdError.Message = 'Ошибка при обращении к серверу. Запрашиваемый ресурс недоступен для пользователя.';
          return asdError;
        case 404:
          asdError.Message = 'Ошибка при обращении к серверу. Запрашиваемый ресурс не найден.';
          return asdError;
        default:
          asdError.Message = 'Неизвестная ошибка при обращении к серверу.';
          return asdError;
      }
    } catch (error) {
      asdError.Message = 'Неизвестная ошибка при обращении к серверу.';
      return asdError;
    }
  }

  // преобразование даты и времени в строковое значение даты
  ToDateString(date: any): any {
    if (date) {
      return moment(date).format('DD MMMM YYYY');
    } else {
      return 'неизвестно';
    }
  }

  // преобразование даты и времени в строковое значение времени
  ToTimeString(date: any): any {
    if (date) {
      return moment(date).format('HH:mm');
    } else {
      return '';
    }
  }

  // получение данных
  Get(url: any, id?: any): Observable<any> {
    // формируем заголовки запроса
    const headers = this.GetApiHeaders();
    // проверяем указан ли id
    if (id) {
      // если id  указан, то получаем один объект
      return this.httpClient
        .get(this.GetApiUrl() + '/' + url.toLowerCase() + '/' + id.toLowerCase(), { headers })
        .pipe(map((data: any) => {
          // сохраняем correlationId на стороне клиента
          this.SetСorrelationId(data.CorrelationId);
          // если код ошибки 00100, то переходим на начальную страницу
          if (data.ErrorCode === '00100') { this.router.navigate(['/home']); }
          // возвращаем результат
          return data;
        }), catchError(error => {
          console.log('ERROR DataService.get ', error);
          return throwError(this.GetError(error));
        }));
    } else {
      // если id не указан, то получаем список объектов
      return this.httpClient
        .get(this.GetApiUrl() + '/' + url.toLowerCase(), { headers })
        .pipe(map((data: any) => {
          // сохраняем correlationId на стороне клиента
          this.SetСorrelationId(data.CorrelationId);
          // если код ошибки 00100, то переходим на начальную страницу
          if (data.ErrorCode === '00100') { this.router.navigate(['/home']); }
          // возвращаем результат
          return data;
        }), catchError(error => {
          console.log('ERROR DataService.get ', error);
          return throwError(this.GetError(error));
        }));
    }
  }

}
