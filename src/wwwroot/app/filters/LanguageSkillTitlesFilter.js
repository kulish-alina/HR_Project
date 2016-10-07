import { map } from 'lodash';

export default function languageSkillsTitlesFilterCreator() {
   return function _getLanguageSkillsTitles(languageSkills) {
      return map(languageSkills, languageSkill => {
         if (languageSkill.language && languageSkill.languageLevel) {
            return languageSkill.languageLevel ?
               `${languageSkill.language.title} ${languageSkill.languageLevel.title}` :
               languageSkill.language.title;
         }
         return '';
      });
   };
}
