const THESAURUS_STRUCTURES = {
   'country' :  {
      thesaurusName : 'THESAURUSES.COUNTRIES',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'name',      type : 'text',   validator : 'required, maxlength=50, minlength=3'},
         {name : 'state',     label : 'state',     type : ''}
      ],
      orderFieldName : 'title'
   },
   'socialnetwork' : {
      thesaurusName : 'THESAURUSES.SOCIALS',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'name',      type : 'text',    validator : 'required'},
         {name : 'imagePath', label : 'image',     type : 'img' },
         {name : 'state',     label : 'state',     type : ''}
      ],
      orderFieldName : 'title'
   },
   'language' : {
      thesaurusName : 'THESAURUSES.LANGUAGES',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=50, minlength=3'},
         {name : 'state',     label : 'state',     type : ''}
      ],
      orderFieldName : 'title'
   },
   'city' : {
      thesaurusName : 'THESAURUSES.LOCATIONS',
      fields : [
         {name : 'id',              label : 'id',                    type : ''},
         {name : 'title',           label : 'name',                  type : 'text',
          validator : 'required, maxlength=50, minlength=3'},
         {name : 'countryId',         label : 'country',               type : 'select',
          refTo : 'country',      labelRefFieldName : 'title',     refObject : 'countryObject'},
         {name : 'hasOffice',       label : 'has office',            type : 'checkbox'},
         {name : 'state',           label : 'state',                 type : ''}
      ],
      orderFieldName : 'title'
   },
   'department' : {
      thesaurusName : 'THESAURUSES.DEPARTMENTS',
      fields : [
         {name : 'id',                    label : 'id',                    type : ''},
         {name : 'title',                 label : 'name',                  type : 'text',
          validator : 'required, maxlength=50'},
         {name : 'departmentGroupId',     label : 'department group',      type : 'select',
          refTo : 'departmentgroup',     labelRefFieldName : 'title',     refObject : 'departmentGroupObject'},
         {name : 'state',                 label : 'state',                 type : ''}
      ],
      orderFieldName : 'title'
   },
   'departmentgroup' : {
      thesaurusName : 'THESAURUSES.DEPARTMENT_GROUPS',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=50, minlength=3'},
         {name : 'state',     label : 'state',     type : ''}
      ],
      orderFieldName : 'title'
   },
   'industry' : {
      thesaurusName : 'THESAURUSES.INDUSTRIES',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=50, minlength=3'},
         {name : 'state',     label : 'state',     type : ''}
      ],
      orderFieldName : 'title'
   },
   'skill' : {
      thesaurusName : 'THESAURUSES.SKILLS',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=50'},
         {name : 'state',     label : 'state',     type : ''}
      ],
      orderFieldName : 'title'
   },
   'tag' : {
      thesaurusName : 'THESAURUSES.TAGS',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=50'},
         {name : 'state',     label : 'state',     type : ''}
      ],
      orderFieldName : 'title'
   },
   'level' : {
      thesaurusName : 'THESAURUSES.LEVELS',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=50, minlength=3'},
         {name : 'state',     label : 'state',     type : ''}
      ],
      orderFieldName : 'title'
   },
   'eventtype' : {
      thesaurusName : 'THESAURUSES.EVENT_TYPES',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'name',      type : 'text',    validator : 'required, maxlength=50, minlength=3'},
         {name : 'imagePath', label : 'image',     type : 'img' },
         {name : 'state',     label : 'state',     type : ''}
      ],
      orderFieldName : 'title'
   },
   'currency' : {
      thesaurusName : 'THESAURUSES.CURRENCY',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=3, minlength=3'},
         {name : 'state',     label : 'state',     type : ''}
      ],
      orderFieldName : 'title'
   },
   'source' : {
      thesaurusName : 'THESAURUSES.SOURCES',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required'},
         {name : 'state',     label : 'state',     type : ''}
      ],
      orderFieldName : 'title'
   }
};

export default THESAURUS_STRUCTURES;
