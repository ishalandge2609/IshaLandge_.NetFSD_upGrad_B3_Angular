import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './footer.component.html'
})
export class FooterComponent {

  private toastr = inject(ToastrService);

  subscribe(email: string) {

    if (email) {

      this.toastr.success(
        `Thanks for subscribing with ${email}!`
      );

    }

  }

}