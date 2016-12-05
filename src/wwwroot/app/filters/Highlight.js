export default function() {
   return function Highlight(text, className = 'highlight-filter') {
      return `<span class="${className}">${text}</span>`;
   };
}
