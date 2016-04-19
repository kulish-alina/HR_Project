export const THESAURUS_STRUCTURES = {
   'countries' :  {
      thesaurusName : 'THESAURUSES.COUNTRIES',
      fields : [
         {name : 'id', label : 'id', type : ''},
         {name : 'name', label : 'name', type : 'text'},
         {name : 'editTime', label : 'editTime', type : ''},
         {name : 'state', label : 'state', type : ''}
      ]
   },
   'socialnetworks' : {
      thesaurusName : 'THESAURUSES.SOCIALS',
      fields : [
         {name : 'id', label : 'id', type : ''},
         {name : 'title', label : 'name', type : 'text' },
         {name : 'imagePath', label : 'image', type : 'img' },
         {name : 'editTime', label : 'editTime', type : ''},
         {name : 'state', label : 'state', type : ''}
      ]
   },
   'languages' : {
      thesaurusName : 'THESAURUSES.LANGUAGES',
      fields : [
         {name : 'id', label : 'id', type : ''},
         {name : 'title', label : 'title', type : 'text' },
         {name : 'editTime', label : 'editTime', type : ''},
         {name : 'state', label : 'state', type : ''}
      ]
   },
   'cities' : {
      thesaurusName : 'THESAURUSES.LOCATIONS',
      fields : [
         {name : 'id', label : 'id', type : ''},
         {name : 'name', label : 'name', type : 'text' },
         {name : 'country', label : 'country', type : 'select',
           refTo : 'countries', labelRefFieldName : 'name', additionFieldForText : 'countryName'},
         {name : 'editTime', label : 'editTime', type : ''},
         {name : 'state', label : 'state', type : ''}
      ]
   }
};
