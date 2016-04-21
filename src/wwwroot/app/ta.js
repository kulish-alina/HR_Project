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



// use as: addCSSRule("header", "float: left");
window.addCSSRule = function(selector, rules) {
   return _addCSSRule(sheet, selector, rules);
};
window._addCSSRule = function(_sheet, selector, rules){
   var insertIndex;
   var insertedRule;
   // This order is important as IE 11 has both cssRules and rules but they have different lengths - cssRules is correct, rules gives an error in IE 11
   /* istanbul ignore next: browser catches */
   if(_sheet.cssRules) insertIndex = Math.max(_sheet.cssRules.length - 1, 0);
   else if(_sheet.rules) insertIndex = Math.max(_sheet.rules.length - 1, 0);

   /* istanbul ignore else: untestable IE option */
   if(_sheet.insertRule) {
      _sheet.insertRule(selector + "{" + rules + "}", insertIndex);
   }
   else {
      _sheet.addRule(selector, rules, insertIndex);
   }
   /* istanbul ignore next: browser catches */
   if(sheet.rules) insertedRule = sheet.rules[insertIndex];
   else if(sheet.cssRules) insertedRule = sheet.cssRules[insertIndex];
   // return the inserted stylesheet rule
   return insertedRule;
};

window._getRuleIndex = function(rule, rules) {
   var i, ruleIndex;
   for (i=0; i < rules.length; i++) {
      /* istanbul ignore else: check for correct rule */
      if (rules[i].cssText === rule.cssText) {
         ruleIndex = i;
         break;
      }
   }
   return ruleIndex;
};

window.removeCSSRule = function(rule){
   _removeCSSRule(sheet, rule);
};
/* istanbul ignore next: tests are browser specific */
window._removeCSSRule = function(sheet, rule){
   var rules = sheet.cssRules || sheet.rules;
   if(!rules || rules.length === 0) return;
   var ruleIndex = _getRuleIndex(rule, rules);
   if(sheet.removeRule){
      sheet.removeRule(ruleIndex);
   }else{
      sheet.deleteRule(ruleIndex);
   }
};
