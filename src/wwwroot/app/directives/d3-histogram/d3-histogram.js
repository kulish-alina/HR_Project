import template from './d3-histogram.directive.html';
import './d3-histogram.scss';
import {
   map,
   flatten,
   uniqBy,
   sumBy,
   round,
   isEmpty,
   keys,
   each,
   some
} from 'lodash';

const elementColors = ['#00838F', '#0097A7', '#00ACC1',
                      '#00BCD4', '#26C6DA', '#4DD0E1', '#80DEEA', '#B2EBF2', '#E0F7FA'];
const columnColorOnHover = '#006064';
const maxTicksCount = 4;
const axisStepDivider = 100;
const ticsCountDivider = 10;

export default class HistogramDirective {
   constructor() {
      this.restrict   = 'E';
      this.template   = template;
      this.controller = HistogramController;
   }
   static createInstance() {
      'ngInject';
      HistogramDirective.instance = new HistogramDirective();
      return HistogramDirective.instance;
   }
}

function HistogramController($scope, $translate, UserDialogService, D3Service) {
   let d3 = D3Service.getD3();
   let vm = $scope;
   vm.generateHistogram = generateHistogram;
   vm.$on('onGenerateReport', function fromParent(event, obj) {
      clearContainer();
      if (keys(obj).length > 1) {
         let index = 0;
         each(obj.value, (value, key) => {
            d3.select('.histogram-container')
               .append('div')
               .attr('class', key);
            let classContainer = `.${key}`;
            generateHistogram(value, classContainer, obj.titles[index]);
            index++;
         });
      } else {
         generateHistogram(obj.value, '.histogram-container');
      }
   });
   vm.$on('onClear', function fromParentClear() {
      clearContainer();
   });

   function clearContainer() {
      d3.select('.histogram-container').selectAll('div').remove();
      d3.select('.histogram-container').selectAll('svg').remove();
   }

   function generateHistogram(data, container, title) {
      let margin = {
         top: 20,
         right: 20,
         bottom: 20,
         left: 20
      };
      let containerWidth = (window.screen.width / 12) * 10;
      let height = 400 - margin.top - margin.bottom;
      let width = containerWidth - margin.left - margin.right;

      function _isDataEmpty(value) {
         return isEmpty(value) || !some(value, x => x.values.length !== 0);
      }

      //create svg element for empty report chart
      if (_isDataEmpty(data)) {
         height = 20;
         margin = {
            top: 8,
            right: 16,
            bottom: 8,
            left: 16
         };
         let svgForEmptyData = d3.select(container)
            .append('svg')
            .attr('width', width + margin.left + margin.right)
            .attr('height', height + margin.top + margin.bottom)
            .append('text')
            .text($translate.instant('REPORTS.NO_INFORMATION'))
            .attr('transform', `translate( 0, ${margin.top * 2} )`);
         return svgForEmptyData;
      }

      let maxValue = d3.max(data, (c) => {
         return d3.max(c.values, (d) => {
            return d.value;
         });
      });

      let axisStep = maxValue <= axisStepDivider ? 10 :
                     ((maxValue +  (axisStepDivider - maxValue % axisStepDivider)) / axisStepDivider) * 10;
      let ticsCount = maxValue <= axisStep ? maxValue :
                     (maxValue + (axisStep - maxValue % ticsCountDivider)) / axisStep;

      let keysArr = map(data, d => {
         return map(d.values, v => {
            return v.name;
         });
      });

      let groupKeys = uniqBy(flatten(keysArr), (e) => {
         return e;
      });

      let x0 = d3.scaleBand().rangeRound([0, width - 100]).padding(0.1);

      let x1 = d3.scaleBand().rangeRound([0, width]);

      let y = d3.scaleLinear()
         .range([height, 2 * margin.top]);

      let color = d3.scaleOrdinal()
         .range(elementColors);

      let svg = d3.select(container).append('svg')
        .attr('width', width + margin.left + margin.right)
        .attr('height', height + margin.top + margin.bottom)
        .append('g')
        .attr('transform', `translate( ${margin.top + margin.right}, -${margin.top} )`);

      svg.append('text')
        .attr('x', (width - 100) / 2)
        .attr('y', 35)
        .style('text-decoration', 'underline')
        .style('font-weight', '600')
        .style('font-size', '12.8px')
        .text(title);

      let yAxis = d3.axisLeft(y)
         .ticks(ticsCount);

      let xAxis = d3.axisBottom(x0)
        .ticks(data.length);

      let tooltip = d3.select(container).append('div').attr('class', 'toolTip');

      x0.domain(data.map((d) => {
         return d.state;
      }));

      x1.domain(groupKeys).range([0, x0.bandwidth()]);

      y.domain([0, maxValue]);

      svg.append('g')
        .attr('class', 'x axis')
        .attr('transform', `translate( 0, ${height} )`)
        .call(xAxis)
        .selectAll('text')
        .attr('y', 10)
        .attr('x', 3)
        .attr('dy', '.35em')
        .attr('transform', (elem, index, arr) => {
           return arr.length > maxTicksCount ? 'rotate(25)' : 'rotate(0)';
        })
        .style('text-anchor', (elem, index, arr) => {
           return arr.length > maxTicksCount ? 'start' : 'center';
        });

      svg.append('g')
        .attr('class', 'y axis')
        .call(yAxis)
        .append('text')
        .attr('transform', 'rotate(-90)')
        .attr('y', 6)
        .attr('dy', '.71em')
        .style('text-anchor', 'end')
        .text('Candidates count');

      let state = svg.selectAll('.state')
        .data(data)
        .enter().append('g')
        .attr('class', 'state')
        .attr('transform', (d) => {
           return `translate( ${x0(d.state)}, 0 )`;
        });

      state.selectAll('rect')
         .data((d) => {
            return d.values;
         })
        .enter().append('rect')
        .attr('width', x1.bandwidth())
        .attr('class', 'column')
        .attr('x', (d) => {
           return x1(d.name);
        })
        .attr('y', (d) => {
           return y(d.value);
        })
        .attr('height', (d) => {
           return height - y(d.value);
        })
        .style('fill', (d) => {
           return color(d.name);
        })
        .on('mousemove', (d, index, columnArr) => {
           d3.select(columnArr[index]).style('fill', columnColorOnHover);
           let coords = d3.mouse(d3.select('.histogram-container').node());
           tooltip
              .style('left', `${coords[0] + 25}px`)
              .style('top', `${coords[1]}px`)
              .style('display', 'inline-block')
              .html(`${d.name}: ${d.value}`);
        })
        .on('mouseout', (d, index, columnArr) => {
           d3.select(columnArr[index]).style('fill', (element) => {
              return color(element.name);
           });
           tooltip.style('display', 'none');
        })
        .on('click', (d) => {
           if (!isEmpty(d.users)) {
              generatePie(d.name, d.users);
           }
        });

      let legend = svg.selectAll('.legend')
        .data(groupKeys.slice())
        .enter().append('g')
        .attr('class', 'legend')
        .attr('transform', (d, i) => {
           return `translate( -100, ${20 + i * margin.right} )`;
        });

      legend.append('rect')
        .attr('x', width - 18)
        .attr('width', 18)
        .attr('height', 18)
        .style('fill', color)
        .on('mousemove', (groupName) => {
           let p = d3.selectAll('.column').filter((val) => {
              if (val.name === groupName) {
                 return val;
              }
           });
           p.style('fill', columnColorOnHover);
        })
        .on('mouseout', (groupName) => {
           let p = d3.selectAll('.column').filter((val) => {
              if (val.name === groupName) {
                 return val;
              }
           });
           p.style('fill', (element) => {
              return color(element.name);
           });
        });

      legend.append('text')
        .attr('x', width - 24)
        .attr('y', 9)
        .attr('dy', '.35em')
        .style('text-anchor', 'end')
        .text((d) => {
           return d;
        });
   }

   function generatePie(location, pieData) {
      let pieKeys = map(pieData, (d) => {
         return d.name;
      });
      let sumValues = sumBy(pieData, (d) => {
         return d.number;
      });

      let width = 680,
         height = 450,
         radius = Math.min(width, height) / 2;

      let color = d3.scaleOrdinal()
         .range(elementColors);

      let arc = d3.arc()
         .outerRadius(radius - 10)
         .innerRadius(30);

      let labelArc = d3.arc()
         .outerRadius(radius - 40)
         .innerRadius(radius - 40);

      let pie = d3.pie()
         .sort(null)
         .value((d) => {
            return d.number;
         });
      let arcs = pie(pieData);

      let svg = d3.select('.pie-container').append('svg')
         .attr('class', 'pie-svg-cont')
         .attr('width', width)
         .attr('height', height)
         .append('g')
         .attr('transform', `translate( ${width - 420}, ${height / 2} )`);

      let g = svg.selectAll('.arc')
         .data(arcs)
         .enter()
         .append('g')
         .attr('class', 'arc');

      g.append('path')
      .attr('d', arc)
      .style('fill', (d) => {
         return color(d.index);
      });

      g.append('text')
      .attr('transform', (d) => {
         return `translate( ${labelArc.centroid(d)} )`;
      })
      .attr('class', 'pie-piece-value')
      .attr('dy', '.35em')
      .text((d) => {
         return `${round((d.value / sumValues) * 100)}%`;
      });

      let legend = svg.selectAll('.legend')
        .data(pieKeys)
        .enter().append('g')
        .attr('class', 'legend')
        .attr('transform', (d, i) => {
           return `translate( 160, ${(i * 20) - 200} )`;
        });

      legend.append('rect')
        .attr('x', width / 3)
        .attr('y', 0)
        .attr('width', 18)
        .attr('height', 18)
        .style('fill', (element, i) => {
           return color(i);
        });

      legend.append('text')
        .attr('class', 'pie-legend-text')
        .attr('x', (width / 3) - 18)
        .attr('y', 9)
        .attr('dy', '.35em')
        .style('text-anchor', 'end')
        .text((d, i) => {
           return `${d}: ${pieData[i].number}`;
        });
      //open modal window afrer creating svg element with pie chart
      openModalWindowForPieChart(location);
      return svg;
   }

   function openModalWindowForPieChart(title) {
      let pieElement = document.getElementById('pie-cont').innerHTML;
      let buttons = [
         {
            name: $translate.instant('COMMON.CLOSE'),
            func: () =>  {
               document.getElementById('pie-cont').innerHTML = '';
            }
         }
      ];
      UserDialogService.dialog(title, pieElement, buttons, null, false);
   }
}
