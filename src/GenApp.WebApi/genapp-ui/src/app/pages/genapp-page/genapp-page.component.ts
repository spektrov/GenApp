import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { GenappService } from '../../services/genapp.service';
import { CommonModule } from '@angular/common';
import { DbmsType } from '../../models/dbmsType';
import { FileDataRequest } from '../../models/fileDataRequest';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-genapp-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  providers: [GenappService],
  templateUrl: './genapp-page.component.html',
  styleUrl: './genapp-page.component.css'
})
export class GenappPageComponent {
  isLoading = false;

  dbmsTypes = Object.entries(DbmsType).map(([key, value]) => ({key, value}));
  selectedFile: File | null = null; 

  form: FormGroup;

  constructor(private genappService: GenappService, private formBuilder: FormBuilder) {
    this.form = this.formBuilder.group({
      dbmsType: ['', [Validators.required]],
      appName: ['', [Validators.required]],
      dotnetSdkVersion: ['8', [Validators.required]],
      useDocker: [false, [Validators.required]],
      connectionString: [null]
    }, { validator: this.dockerOrConnectionStringValidator });
  }

  ngOnInit(): void {
  }

  onFileChange(event: any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      if (this.fileValidator(file)) {
          this.selectedFile = file;
      } else {
          event.target.value = '';
          this.selectedFile = null;
      }
    } else {
      this.selectedFile = null;
    }
  }
  
  onFormSubmit() {
    if (this.form.valid && this.selectedFile) {
      let request : FileDataRequest = {
        file: this.selectedFile,
        dbmsType: this.form.get('dbmsType')?.value,
        appName: this.form.get('appName')?.value,
        dotnetSdkVersion: this.form.get('dotnetSdkVersion')?.value,
        useDocker: this.form.get('useDocker')?.value,
        connectionString: !this.form.get('useDocker')?.value ? this.form.get('connectionString')?.value : null
      }

      this.isLoading = true;
      this.genappService.generateWebApp(request)
      .pipe(finalize(() => this.isLoading = false))
      .subscribe((blob: Blob) => {
        this.genappService.downloadFile(blob, request.appName);
      });
    }
  }

  showUseDocker() : boolean {
    let dbms = this.form.get('dbmsType')?.value;
    if (dbms?.length > 0) {
      return dbms == 'MYSQL' || dbms == 'POSTGRESQL';
    }
    return false;
  }

  showConnectionString() : boolean {
    let dbms = this.form.get('dbmsType')?.value;
    if (dbms?.length > 0) {
      let useDocker = this.form.get('useDocker')?.value;
      return !useDocker;
    }
    return false;
  }

  private fileValidator(file: File): boolean {
    const valid = file &&
    ['text/plain', 'application/sql'].includes(file.type) &&
    file.size <= 100000;

    return valid;
  }

  private dockerOrConnectionStringValidator(formGroup: FormGroup) {
    const useDocker = formGroup.get('useDocker')?.value;
    const connectionString = formGroup.get('connectionString')?.value;
    if (useDocker === true || (connectionString && connectionString.trim() !== '')) {
        return null;
    } else {
        return { dockerOrConnectionString: true };
    }
}
}
