import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
// import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {

   private http = inject(HttpClient) ;
  
  //protected readonly title = signal('client');
  protected readonly title = 'Dating App';

protected members = signal<any>([]);


   ngOnInit(): void {
   
    var url = 'https://localhost:5001/api/members';
    
    this.http.get(url).subscribe({
      next: response => this.members.set(response)  ,
      error: error => console.log(error),
      complete: () => console.log("Completed http request")
    });

  }
}
