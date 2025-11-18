// =============================================================
// assets/js/my_books.js - Refactored version
// =============================================================

// --- SAMPLE BOOK DATA ---
const books = [
  { 
    id: 1, title: "Frankenstein", author: "Mary Shelley", rating: 4, 
    cover: "https://covers.openlibrary.org/b/id/8231856-L.jpg",
    dateRead: "2024-02-15", shelves: ["read", "classics", "favorites"]
  },
  { 
    id: 2, title: "The Great Gatsby", author: "F. Scott Fitzgerald", rating: 4, 
    cover: "https://d28hgpri8am2if.cloudfront.net/book_images/onix/cvr9781524879761/the-great-gatsby-9781524879761_hr.jpg",
    dateRead: "2024-01-22", shelves: ["read", "classics"]
  },
  { 
    id: 3, title: "Pride and Prejudice", author: "Jane Austen", rating: 5, 
    cover: "https://i.ebayimg.com/images/g/k9UAAeSwb0Jos~6g/s-l1600.webp",
    dateRead: "2023-12-10", shelves: ["read", "classics", "favorites"]
  },
  { 
    id: 4, title: "1984", author: "George Orwell", rating: 5, 
    cover: "https://covers.openlibrary.org/b/id/7222246-L.jpg",
    dateRead: "2024-03-05", shelves: ["read", "classics"]
  },
  { 
    id: 5, title: "To the Lighthouse", author: "Virginia Woolf", rating: 0, 
    cover: "https://covers.openlibrary.org/b/id/8231991-L.jpg",
    dateRead: "", shelves: ["currently-reading", "fiction"]
  },
  { 
    id: 6, title: "The Hobbit", author: "J. R. R. Tolkien", rating: 0, 
    cover: "https://covers.openlibrary.org/b/id/6979861-L.jpg",
    dateRead: "", shelves: ["to-read", "fiction", "fantasy"]
  },
  { 
    id: 7, title: "The Martian", author: "Andy Weir", rating: 4, 
    cover: "https://covers.openlibrary.org/b/id/8228690-L.jpg",
    dateRead: "2024-03-20", shelves: ["read", "science-fiction"]
  }
];

// --- HELPER FUNCTIONS ---
function makeStarSVG(filled = true) {
  const color = filled ? '#f6b438' : '#e0e0e0';
  return `<svg viewBox="0 0 20 20" fill="${color}" width="14" height="14">
    <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.286 3.951a1 1 0 
    00.95.69h4.162c.969 0 1.371 1.24.588 1.81l-3.37 2.448a1 1 0 
    00-.364 1.118l1.286 3.951c.3.921-.755 1.688-1.538 
    1.118l-3.37-2.448a1 1 0 00-1.176 0l-3.37 
    2.448c-.783.57-1.838-.197-1.538-1.118l1.286-3.951a1 
    1 0 00-.364-1.118L2.063 9.378c-.783-.57-.38-1.81.588-1.81h4.162a1 
    1 0 00.95-.69l1.286-3.951z"/>
  </svg>`;
}

function formatDate(dateString) {
  if (!dateString) return "—";
  const date = new Date(dateString);
  return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' });
}

function escapeHtml(s) {
  return String(s).replace(/[&<>"]/g, c => ({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;'}[c]));
}

// --- MAIN RENDERING LOGIC ---
let currentShelf = "all";

function renderBooksTable() {
  const tableBody = document.getElementById("booksTableBody");
  tableBody.innerHTML = "";

  const filtered = books.filter(book =>
    currentShelf === "all" || book.shelves.includes(currentShelf)
  );

  if (filtered.length === 0) {
    tableBody.innerHTML = `<tr><td colspan="5" style="text-align:center; padding:20px;">No books found.</td></tr>`;
    return;
  }

  filtered.forEach(book => {
    const row = document.createElement("tr");
    const stars = book.rating > 0
      ? Array.from({length: 5}, (_, i) => makeStarSVG(i < book.rating)).join('')
      : `<span style="color:var(--muted)">Not rated</span>`;
    const shelfTags = book.shelves.map(s => `<span class="shelf-tag">${s}</span>`).join(' ');

    row.innerHTML = `
      <td>
        <div class="book-title-cell">
          <div class="table-book-cover">
            <img src="${book.cover}" alt="${escapeHtml(book.title)}">
          </div>
          <div class="book-title-info">
            <a href="book.html?id=${book.id}" class="book-link">${escapeHtml(book.title)}</a>
          </div>
        </div>
      </td>
      <td>${escapeHtml(book.author)}</td>
      <td><div class="rating-cell">${stars}</div></td>
      <td>${formatDate(book.dateRead)}</td>
      <td>${shelfTags}</td>
    `;
    tableBody.appendChild(row);
  });
}

function setActiveShelf(shelf) {
  currentShelf = shelf;
  document.querySelectorAll(".shelf-list li").forEach(li =>
    li.classList.toggle("active", li.dataset.shelf === shelf)
  );
  document.getElementById("sectionTitle").textContent =
    shelf === "all" ? "All Books" :
    shelf === "read" ? "Read Books" :
    shelf === "currently-reading" ? "Currently Reading" :
    shelf === "to-read" ? "Want to Read" :
    "Favorite Books";
  renderBooksTable();
}

// --- INIT ---
document.addEventListener("DOMContentLoaded", () => {
  document.querySelectorAll(".shelf-list li").forEach(li => {
    li.addEventListener("click", () => setActiveShelf(li.dataset.shelf));
  });
  setActiveShelf("all");
});
