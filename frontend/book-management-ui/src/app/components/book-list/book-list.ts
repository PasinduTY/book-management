import {
  Component,
  OnInit,
  inject,
  ChangeDetectionStrategy,
  signal,
  computed,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookService } from '../../services/book.service';
import { Book } from '../../models/book';
import { BookFormComponent } from '../book-form/book-form';

@Component({
  selector: 'app-book-list',
  imports: [CommonModule, BookFormComponent],
  templateUrl: './book-list.html',
  styleUrl: './book-list.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BookListComponent implements OnInit {
  private bookService = inject(BookService);

  books = signal<Book[] | null>(null);
  error = signal<string | null>(null);
  editingId = signal<number | null>(null);
  isAdding = signal(false);

  isEditing = computed(() => this.editingId() !== null);

  selectedBook = computed(() => {
    const currentBooks = this.books();
    const currentId = this.editingId();
    if (!currentBooks || currentId === null) return null;
    return currentBooks.find((b) => b.id === currentId) ?? null;
  });

  ngOnInit(): void {
    this.loadBooks();
  }

  loadBooks(): void {
    this.bookService.getBooks().subscribe({
      next: (response) => {
        this.books.set(response.data || response);
        this.error.set(null);
      },
      error: (err) => {
        console.error('Error loading books:', err);
        this.error.set('Failed to load books');
        this.books.set([]);
      },
    });
  }

  startAdd(): void {
    this.isAdding.set(true);
    this.editingId.set(null);
  }

  finishAdd(): void {
    this.isAdding.set(false);
    this.loadBooks();
  }

  editBook(id: number): void {
    this.editingId.set(id);
    this.isAdding.set(false);
  }

  finishEdit(): void {
    this.editingId.set(null);
    this.loadBooks();
  }

  cancelEdit(): void {
    this.editingId.set(null);
  }

  deleteBook(id: number): void {
    this.bookService.deleteBook(id).subscribe({
      next: () => this.loadBooks(),
      error: (err) => {
        console.error('Error deleting book:', err);
        this.error.set('Failed to delete book');
      },
    });
  }
}
