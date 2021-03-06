
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos: any = [];
  eventosFiltrados: any = [];
  widthImg: number = 200;
  marginImg: number = 1;
  showImg = true;
  private _filtroLista: string = '';

  public get filtroLista(): string {
    return this._filtroLista;
  }

  public set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  filtrarEventos(filtrarPor: string): any {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (eventos: any) => eventos.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
                        eventos.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getEventos()
  }

  public showImage() {
    this.showImg = !this.showImg;
  }


  public getEventos(): void {

    this.http.get(`https://localhost:5001/api/eventos`).subscribe(
      response =>  {
        this.eventos = response
        this.eventosFiltrados = this.eventos
      },
      error => console.log(error)
    )
  }
}
