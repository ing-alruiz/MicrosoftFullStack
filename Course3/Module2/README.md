# CSS Grid & Flexbox Demo

This project demonstrates the power of combining CSS Grid and Flexbox to create modern, responsive web layouts.

## 🎯 What This Demo Shows

### CSS Grid Features Demonstrated:
1. **Main Layout Structure** - Using CSS Grid to define the overall page layout with `grid-template-areas`
2. **Features Section** - Responsive grid with `auto-fit` and `minmax()` for automatic column sizing
3. **Portfolio Section** - Advanced grid with items spanning multiple columns and rows
4. **Responsive Grid Changes** - How grid layouts adapt on different screen sizes

### Flexbox Features Demonstrated:
1. **Header Navigation** - Horizontal layout with space distribution
2. **Hero Section** - Centering content and creating flexible layouts
3. **Stats Section** - Evenly distributed items with wrapping
4. **Footer** - Complex footer layout with multiple sections
5. **Card Content** - Vertical content alignment within cards

## 🏗️ Layout Structure

```
┌─────────────────────────────────┐
│            HEADER               │ ← Flexbox for navigation
├─────────────────────────────────┤
│             HERO                │ ← Flexbox for content alignment
├─────────────────────┬───────────┤
│      FEATURES       │           │ ← CSS Grid for cards
├─────────────────────┤ SIDEBAR   │ ← Flexbox for news items
│     PORTFOLIO       │           │ ← Advanced Grid layout
├─────────────────────┤           │
│       STATS         │           │ ← Flexbox for even distribution
├─────────────────────┴───────────┤
│            FOOTER               │ ← Flexbox for columns
└─────────────────────────────────┘
```

## 🔧 Key CSS Concepts

### CSS Grid Layout
```css
.grid-container {
    display: grid;
    grid-template-columns: 1fr 300px;
    grid-template-areas: 
        "header header"
        "hero hero"
        "features sidebar"
        "portfolio sidebar"
        "stats sidebar"
        "footer footer";
}
```

### Flexbox for Components
```css
.header-content {
    display: flex;
    justify-content: space-between;
    align-items: center;
}
```

### Responsive Grid
```css
.features-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
    gap: 2rem;
}
```

## 📱 Responsive Design

The layout includes three main breakpoints:
- **Desktop** (1024px+): Full grid layout with sidebar
- **Tablet** (768px-1024px): Stacked layout, modified grid
- **Mobile** (768px and below): Single column, simplified navigation

## 🎨 Design Features

1. **Modern Color Gradients** - Beautiful gradient backgrounds
2. **Hover Effects** - Interactive elements with smooth transitions
3. **Card-based Design** - Clean, modern card layouts
4. **Typography Hierarchy** - Clear content structure
5. **Animations** - Subtle fade-in animations for enhanced UX

## 🚀 How to Use

1. Open `index.html` in a web browser
2. Resize the browser window to see responsive behavior
3. Hover over cards and buttons to see interactive effects
4. Use browser developer tools to inspect the Grid and Flexbox layouts

## 📖 Learning Points

### When to Use CSS Grid:
- ✅ Overall page layout
- ✅ Two-dimensional layouts (rows AND columns)
- ✅ Complex grid systems
- ✅ Overlapping content

### When to Use Flexbox:
- ✅ One-dimensional layouts (either row OR column)
- ✅ Component-level layouts
- ✅ Content alignment and distribution
- ✅ Navigation bars and button groups

### Best Practices Demonstrated:
1. **Mobile-First Approach** - Responsive design starts with mobile
2. **Semantic HTML** - Proper use of HTML5 semantic elements
3. **CSS Organization** - Well-structured and commented CSS
4. **Performance** - Efficient selectors and minimal DOM manipulation
5. **Accessibility** - Proper contrast ratios and semantic structure

## 🔍 Browser Support

- **CSS Grid**: IE 11+ (with -ms- prefixes), All modern browsers
- **Flexbox**: IE 11+, All modern browsers
- **CSS Variables**: IE 11 needs fallbacks, Full support in modern browsers

This demo showcases how CSS Grid and Flexbox complement each other to create powerful, flexible layouts that work across all devices and screen sizes.
