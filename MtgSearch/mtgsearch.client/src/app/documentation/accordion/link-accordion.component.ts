import { Component, EventEmitter, HostBinding, Input, OnInit, Output } from "@angular/core";
import { Subject } from "rxjs";
class Clickable{
  public constructor(public link: string, public active: boolean) { }
}

@Component({
  selector: "link-accordion",
  templateUrl: "./link-accordion.component.html"
})
export class LinkAccordionComponent implements OnInit {
  @Input() @HostBinding("style.--docDisplayPaneBgColor") public dispColor: string = '#FFF';
  @Input() public expanderText: string = "unknown";
  public links: Clickable[] = [];
  @Input() public set linkNames(value: string[]) {
    this.links = value.map(x => new Clickable(x, false));
  }
  @Input() public changeInAccordionListener: Subject<string> = new Subject();
  ngOnInit(): void {
    this.changeInAccordionListener.subscribe(x => {
      if (x != this.expanderText) {
        this.clearAll();
      }
    });
  }

  @Output() selectionChanged = new EventEmitter<string>();

  public toggleAccordion(event: MouseEvent) {
    let element = event?.target as HTMLElement;
    if (!element) return;
    let button = element.closest('button');
    if (!button) return;
    button.classList.toggle('active');
    let children = button.parentElement?.children;
    if (!children || children.length < 2) return;
    children[1].classList.toggle('active');
  }
  private clearAll() {
    for (let link of this.links) {
      link.active = false;
    }
  }
  public clickLink(name: string) {
    this.clearAll();
    let match = this.links.filter(x => x.link == name);
    if (match.length == 1) {
      match[0].active = true;
    }
    this.selectionChanged.emit(name);
  }
}

