const THESAURUS_STRUCTURES = {
   'country' :  {
      thesaurusName : 'THESAURUSES.COUNTRIES',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'name',      type : 'text',   validator : 'required, maxlength=50, minlength=3'},
         {name : 'state',     label : 'state',     type : ''}
      ]
   },
   'socialnetwork' : {
      thesaurusName : 'THESAURUSES.SOCIALS',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'name',      type : 'text',    validator : 'required, maxlength=50, minlength=3'},
         {name : 'imagePath', label : 'image',     type : 'img' },
         {name : 'state',     label : 'state',     type : ''}
      ]
   },
   'language' : {
      thesaurusName : 'THESAURUSES.LANGUAGES',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=50, minlength=3'},
         {name : 'state',     label : 'state',     type : ''}
      ]
   },
   'city' : {
      thesaurusName : 'THESAURUSES.LOCATIONS',
      fields : [
         {name : 'id',              label : 'id',                    type : ''},
         {name : 'title',           label : 'name',                  type : 'text',
          validator : 'required, maxlength=50, minlength=3'},
         {name : 'countryId',         label : 'country',               type : 'select',
          refTo : 'country',      labelRefFieldName : 'title',     refObject : 'countryObject'},
         {name : 'state',           label : 'state',                 type : ''}
      ]
   },
   'department' : {
      thesaurusName : 'THESAURUSES.DEPARTMENTS',
      fields : [
         {name : 'id',                    label : 'id',                    type : ''},
         {name : 'title',                 label : 'name',                  type : 'text',
          validator : 'required, maxlength=50, minlength=3'},
         {name : 'departmentGroupId',     label : 'department group',      type : 'select',
          refTo : 'departmentgroup',     labelRefFieldName : 'title',     refObject : 'departmentGroupObject'},
         {name : 'state',                 label : 'state',                 type : ''}
      ]
   },
   'departmentgroup' : {
      thesaurusName : 'THESAURUSES.DEPARTMENT_GROUPS',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=50, minlength=3'},
         {name : 'state',     label : 'state',     type : ''}
      ]
   },
   'industry' : {
      thesaurusName : 'THESAURUSES.INDUSTRIES',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=50, minlength=3'},
         {name : 'state',     label : 'state',     type : ''}
      ]
   },
   'skill' : {
      thesaurusName : 'THESAURUSES.SKILLS',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=50, minlength=1'},
         {name : 'state',     label : 'state',     type : ''}
      ]
   },
   'tag' : {
      thesaurusName : 'THESAURUSES.TAGS',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=50, minlength=1'},
         {name : 'state',     label : 'state',     type : ''}
      ]
   },
   'stage' : {
      thesaurusName : 'THESAURUSES.STAGES',
      fields : [
         {name : 'order',        label : 'order',     type : 'number'},
         {name : 'id',           label : 'id',        type : ''},
         {name : 'title',        label : 'title',     type : 'text', validator : 'required, maxlength=50, minlength=3'},
         {name : 'state',        label : 'state',     type : ''},
         {name : 'isDefault',          label : 'default',   type : 'checkbox'},
         {name : 'isCommentRequired',  label : 'comment',   type : 'checkbox'}
      ]
   },
   'level' : {
      thesaurusName : 'THESAURUSES.LEVELS',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=50, minlength=3'},
         {name : 'state',     label : 'state',     type : ''}
      ]
   },
   'eventtype' : {
      thesaurusName : 'THESAURUSES.EVENT_TYPES',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'name',      type : 'text',    validator : 'required, maxlength=50, minlength=3'},
         {name : 'imagePath', label : 'image',     type : 'img' },
         {name : 'state',     label : 'state',     type : ''}
      ]
   },
   'currency' : {
      thesaurusName : 'THESAURUSES.CURRENCY',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=3, minlength=3'},
         {name : 'state',     label : 'state',     type : ''}
      ]
   },
   'source' : {
      thesaurusName : 'THESAURUSES.SOURCES',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=3, minlength=3'},
         {name : 'state',     label : 'state',     type : ''}
      ]
   }
};

export default THESAURUS_STRUCTURES;
