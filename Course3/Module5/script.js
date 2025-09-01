// Step 3: Hamburger menu toggle
function toggleMenu() {
  const nav = document.querySelector('header nav');
  nav.classList.toggle('open');
}

// Add hamburger icon and event listener
window.addEventListener('DOMContentLoaded', function() {
  const nav = document.querySelector('header nav');
  const hamburger = document.createElement('button');
  hamburger.className = 'hamburger';
  hamburger.setAttribute('aria-label', 'Toggle navigation menu');
  hamburger.innerHTML = '&#9776;';
  nav.parentNode.insertBefore(hamburger, nav);
  hamburger.addEventListener('click', toggleMenu);
});

// Step 3: Smooth scrolling for nav links
const navLinks = document.querySelectorAll('nav[role="navigation"] a[href^="#"]');
navLinks.forEach(link => {
  link.addEventListener('click', function(e) {
    e.preventDefault();
    const target = document.querySelector(this.getAttribute('href'));
    if (target) {
      target.scrollIntoView({ behavior: 'smooth' });
    }
    // Close menu on mobile after click
    document.querySelector('header nav').classList.remove('open');
  });
});

// Step 4: Filter Projects
function filterProjects(category) {
  const articles = document.querySelectorAll('#projects article');
  articles.forEach(article => {
    if (category === 'all' || article.dataset.category === category) {
      article.style.display = '';
    } else {
      article.style.display = 'none';
    }
  });
}

// Example filter buttons (add these to your HTML as needed)
// <button onclick="filterProjects('all')">All</button>
// <button onclick="filterProjects('web')">Web</button>
// <button onclick="filterProjects('app')">App</button>

// Step 4: Lightbox effect for project images
function openLightbox(src, alt) {
  let modal = document.getElementById('lightbox-modal');
  if (!modal) {
    modal = document.createElement('div');
    modal.id = 'lightbox-modal';
    modal.style.position = 'fixed';
    modal.style.top = '0';
    modal.style.left = '0';
    modal.style.width = '100vw';
    modal.style.height = '100vh';
    modal.style.background = 'rgba(0,0,0,0.8)';
    modal.style.display = 'flex';
    modal.style.alignItems = 'center';
    modal.style.justifyContent = 'center';
    modal.style.zIndex = '1000';
    modal.innerHTML = '<img style="max-width:90vw;max-height:80vh;border-radius:8px;box-shadow:0 2px 8px #000;" alt="">';
    modal.addEventListener('click', () => modal.remove());
    document.body.appendChild(modal);
  }
  const img = modal.querySelector('img');
  img.src = src;
  img.alt = alt;
  modal.style.display = 'flex';
}

document.querySelectorAll('#projects figure img').forEach(img => {
  img.style.cursor = 'pointer';
  img.addEventListener('click', function() {
    openLightbox(this.src, this.alt);
  });
});

// Step 5: Contact form validation
const contactForm = document.querySelector('#contact form');
if (contactForm) {
  contactForm.addEventListener('submit', function(e) {
    e.preventDefault();
    let valid = true;
    const name = contactForm.querySelector('#name');
    const email = contactForm.querySelector('#email');
    const message = contactForm.querySelector('#message');
    [name, email, message].forEach(field => {
      field.style.borderColor = '#ccc';
    });
    if (!name.value.trim()) {
      name.style.borderColor = 'red';
      valid = false;
    }
    if (!email.value.match(/^\S+@\S+\.\S+$/)) {
      email.style.borderColor = 'red';
      valid = false;
    }
    if (!message.value.trim()) {
      message.style.borderColor = 'red';
      valid = false;
    }
    if (!valid) {
      alert('Please fill in all fields correctly.');
      return;
    }
    alert('Thank you for your message!');
    contactForm.reset();
  });
  // Real-time feedback
  contactForm.querySelectorAll('input, textarea').forEach(field => {
    field.addEventListener('input', function() {
      this.style.borderColor = '#ccc';
    });
  });
}

// Step 6: Debugging helpers
console.log('Portfolio interactivity loaded.');
