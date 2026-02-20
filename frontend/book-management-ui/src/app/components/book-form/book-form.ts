import {
  Component,
  inject,
  input,
  output,
  ChangeDetectionStrategy,
  signal,
  computed,
  effect,
} from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BookService } from '../../services/book.service';
import { Book } from '../../models/book';

@Component({
  selector: 'app-book-form',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './book-form.html',
  styleUrl: './book-form.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    '[class.book-form]': 'true',
  },
})
export class BookFormComponent {
  private bookService = inject(BookService);
  private formBuilder = inject(FormBuilder);

  bookAdded = output<void>();
  cancelled = output<void>();

  book = input<Book | null>(null);

  isEditMode = computed(() => (this.book()?.id ?? 0) > 0);
  isSubmitting = signal(false);
  submitError = signal<string | null>(null);

  form = this.formBuilder.group({
    title: ['', [Validators.required, Validators.minLength(2)]],
    author: ['', [Validators.required, Validators.minLength(2)]],
    isbn: ['', [Validators.required]],
    publicationDate: ['', Validators.required],
  });

  constructor() {
    effect(() => {
      const currentBook = this.book();
      if (!currentBook) return;

      const dateValue =
        typeof currentBook.publicationDate === 'string'
          ? currentBook.publicationDate.split('T')[0]
          : currentBook.publicationDate.toISOString().split('T')[0];

      this.form.patchValue({
        title: currentBook.title,
        author: currentBook.author,
        isbn: currentBook.isbn,
        publicationDate: dateValue,
      });
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.submitError.set('Please fill in all required fields correctly');
      return;
    }

    this.isSubmitting.set(true);
    this.submitError.set(null);

    const formValue = this.form.getRawValue();

    const bookData: Book = {
      id: this.book()?.id || 0,
      title: formValue.title ?? '',
      author: formValue.author ?? '',
      isbn: formValue.isbn ?? '',
      publicationDate: new Date(formValue.publicationDate ?? ''),
    };

    const request$ = this.isEditMode()
      ? this.bookService.updateBook(bookData.id, bookData)
      : this.bookService.addBook(bookData);

    request$.subscribe({
      next: () => {
        this.form.reset();
        this.bookAdded.emit();
        this.isSubmitting.set(false);
      },
      error: (err) => {
        console.error('Error saving book:', err);
        this.submitError.set(`Failed to ${this.isEditMode() ? 'update' : 'add'} book`);
        this.isSubmitting.set(false);
      },
    });
  }

  onCancel(): void {
    this.form.reset();
    this.submitError.set(null);
    this.cancelled.emit();
  }
}
