import 'textAngular/src/textAngularSetup';
import 'textAngular/src/textAngular-sanitize';
import 'textAngular/src/main';
import 'textAngular/src/factories';
import 'textAngular/src/DOM';
import 'textAngular/src/validators';
import 'textAngular/src/taBind';

import rangy from 'rangy';

window.taTools = {};
window._browserDetect = {};

window.BLOCKELEMENTS = /^(address|article|aside|audio|blockquote|canvas|dd|div|dl|fieldset|figcaption|figure|footer|form|h1|h2|h3|h4|h5|h6|header|hgroup|hr|noscript|ol|output|p|pre|section|table|tfoot|ul|video)$/i; // eslint-disable-line max-len
window.LISTELEMENTS = /^(ul|li|ol)$/i;
window.VALIDELEMENTS = /^(address|article|aside|audio|blockquote|canvas|dd|div|dl|fieldset|figcaption|figure|footer|form|h1|h2|h3|h4|h5|h6|header|hgroup|hr|noscript|ol|output|p|pre|section|table|tfoot|ul|video|li)$/i; // eslint-disable-line max-len

window.rangy = rangy;
