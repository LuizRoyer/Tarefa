import { Component, OnInit, ViewChild } from '@angular/core';
import { Tarefa } from './Models/Tarefa';
import { HttpClient } from '@angular/common/http';
import { isNullOrUndefined } from 'util';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
  title = 'NOVA TAREFA';
  tarefa: Tarefa;
  listaTarefas: any = [];
  checked = false;
  excluir = "/assets/img/excluir.jpg";

  constructor(private http: HttpClient) { }
  ngOnInit(): void {
    this.tarefa = new Tarefa();
    this.CarregarLista();
  }

  Save(obj: Tarefa) {
    let mensagem: string;
    if (isNullOrUndefined(obj.id)) {
      obj.status = 1;
      obj.id = 0;
      mensagem = "Cadastrado com Sucesso"
    } else {
      mensagem = "Alterado com Sucesso"
    }
    console.log(this.checked);
    if (this.checked)
      obj.status = 4;

    this.http.post("https://localhost:44321/api/Tarefa/SaveTarefa", obj).subscribe(() => {
    }, error => console.log(error));

    confirm(mensagem);
    this.Novo();
  }
  Novo(): void {
    this.title = 'NOVA TAREFA';
    this.checked = false;

    this.tarefa = new Tarefa();
    this.CarregarLista();
  }
  Editar(id: Number) {
    this.title = 'EDITAR TAREFA';
    this.tarefa = this.listaTarefas.filter((t: Tarefa) => t.id == id)[0];
    this.tarefa.status = 2;
  }
  Deletar(id: Number) {
    this.http.delete("https://localhost:44321/api/Tarefa/RemoveTarefa/" + id).subscribe(() => {
    }, error => console.log(error));
    confirm("Excluido com Sucesso");
    this.Novo();
  }
  //Busca A Lista de Tarefas do Banco de Dados
  CarregarLista() {
    this.http.get("https://localhost:44321/api/Tarefa/SelectTarefa").subscribe(result => {
      this.listaTarefas = result;
    }, error => console.log(error))
  }
  // Metodo desenvolvido para buscar a Imagem do Status
  Status(id: number): string {

    if (id == 1)
      return "/assets/img/novo.jpg"
    if (id == 2)
      return "/assets/img/edit.jpg"
    if (id == 4)
      return "/assets/img/concluido.jpg"
  }
  // Metodo desenvolvido para Identificar a Opcao do titulo 
  Opcao(id: number): string {

    if (id == 1)
      return "Novo"
    if (id == 2)
      return "Editado"
    if (id == 4)
      return "Concluído"
  }
  // Metodo desenvolvido para capturar a conclusão da Tarefa
  Checked(): void {

    if (this.checked) {
      this.checked = false;
    } else {
      this.checked = true;
    }
  }
}
