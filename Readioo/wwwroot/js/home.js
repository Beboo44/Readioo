// ============================================================
// assets/js/home.js
// ============================================================

const books = [
    { id: 1, title: "Frankenstein", author: "Mary Shelley", rating: 4, cover: "https://covers.openlibrary.org/b/id/8231856-L.jpg" },
    { id: 2, title: "The Hobbit", author: "J.R.R. Tolkien", rating: 5, cover: "https://covers.openlibrary.org/b/id/6979861-L.jpg" },
    { id: 3, title: "Pride and Prejudice", author: "Jane Austen", rating: 5, cover: "https://i.ebayimg.com/images/g/k9UAAeSwb0Jos~6g/s-l1600.webp" },
    { id: 4, title: "Dune", author: "Frank Herbert", rating: 5, cover: "https://covers.openlibrary.org/b/id/8128696-L.jpg" },
    { id: 5, title: "The Martian", author: "Andy Weir", rating: 4, cover: "https://covers.openlibrary.org/b/id/8228690-L.jpg" },
    { id: 6, title: "The Great Gatsby", author: "F. Scott Fitzgerald", rating: 4, cover: "https://d28hgpri8am2if.cloudfront.net/book_images/onix/cvr9781524879761/the-great-gatsby-9781524879761_hr.jpg" }
];

function makeStarSVG(filled = true) {
    const color = filled ? '#f6b438' : '#e0e0e0';
    return `<svg viewBox="0 0 20 20" fill="${color}" xmlns="http://www.w3.org/2000/svg">
    <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.286 3.951a1 1 0 
    00.95.69h4.162c.969 0 1.371 1.24.588 1.81l-3.37 2.448a1 1 0 
    00-.364 1.118l1.286 3.951c.3.921-.755 1.688-1.538 
    1.118l-3.37-2.448a1 1 0 00-1.176 0l-3.37 
    2.448c-.783.57-1.838-.197-1.538-1.118l1.286-3.951a1 
    1 0 00-.364-1.118L2.063 9.378c-.783-.57-.38-1.81.588-1.81h4.162a1 
    1 0 00.95-.69l1.286-3.951z"/>
  </svg>`;
}

function renderSectionBooks(sectionId, bookList) {
    const container = document.getElementById(sectionId);
    if (!container) return; // Safety check

    container.innerHTML = "";
    bookList.forEach(book => {
        const div = document.createElement("div");
        div.className = "book-card";
        const stars = Array.from({ length: 5 }, (_, i) => makeStarSVG(i < book.rating)).join('');
        div.innerHTML = `
      <div class="book-cover"><img src="${book.cover}" alt="${book.title}"></div>
      <div class="book-info">
        <div class="book-title"><a href="book.html?id=${book.id}" class="book-link">${book.title}</a></div>
        <div class="book-author">${book.author}</div>
        <div class="book-rating">${stars}</div>
      </div>
    `;
        container.appendChild(div);
    });
}

// --- Init ---
document.addEventListener("DOMContentLoaded", () => {
    // 1. Render Trending (Still Static for now)
    const trending = books.slice(0, 4);
    renderSectionBooks("trendingBooks", trending);

    // 2.  REMOVE THIS! Do not render recent books from JS.
    // The server (C#) is now handling this part.
    // const recent = books.slice(2, 6);
    // renderSectionBooks("recentBooks", recent); 
});